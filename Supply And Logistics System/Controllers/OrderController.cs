using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Supply_And_Logistics_System.Data;
using Supply_And_Logistics_System.Models.Orders;
using Supply_And_Logistics_System.Models.Orders.States;
using Supply_And_Logistics_System.Models.Payments;
using Supply_And_Logistics_System.Models.Shipping;
using Supply_And_Logistics_System.Models.Shipping.Decorators;
using Supply_And_Logistics_System.Models.Products;
using Supply_And_Logistics_System.Services;

namespace Supply_And_Logistics_System.Controllers
{
    /// <summary>
    /// Sipariş oluşturma, ödeme, kargo ve iptal süreçlerini yöneten ana kontrolcü.
    /// Bu sınıf Factory, Strategy, Decorator ve State desenlerinin entegrasyon noktasıdır.
    /// </summary>
    public class OrderController : Controller
    {
        private readonly AppDbContext _context;
        private readonly InventoryService _inventoryService;

        public OrderController(AppDbContext context)
        {
            _context = context;
            _inventoryService = new InventoryService(_context);
        }

        // Ödeme ve onay sayfasını görüntüler. Sepet verilerini ve ürün bileşenlerini yükler.
        [HttpGet]
        public IActionResult Checkout()
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
                return RedirectToAction("Login", "Account");

            var cart = _context.Carts
                .Include(c => c.Items)
                .ThenInclude(i => i.Product)
                .FirstOrDefault(c => c.UserId == userId.Value);

            if (cart == null || !cart.Items.Any())
                return RedirectToAction("Index", "Cart");

            LoadCompositeComponents(cart);

            return View(cart);
        }

        // Sipariş verme işlemini gerçekleştirir. Stok kontrolü, kargo hesaplama ve ödeme yönetimini yapar.
        [HttpPost]
        public IActionResult Checkout(
            string paymentType,
            string carrierType,
            bool addInsurance,
            bool addFragileProtection,
            string? cardNumber,
            string? cardHolder,
            string? expiryDate,
            string? cvv)
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
                return RedirectToAction("Login", "Account");

            var cart = _context.Carts
                .Include(c => c.Items)
                .ThenInclude(i => i.Product)
                .FirstOrDefault(c => c.UserId == userId.Value);

            if (cart == null || !cart.Items.Any())
                return Content("Cart empty.");

            LoadCompositeComponents(cart);

            // 1. STOK KONTROLÜ
            foreach (var item in cart.Items)
            {
                if (item.Product.GetStock() < item.Quantity)
                    return Content($"Not enough stock for {item.Product.Name}");
            }

            // 2. SİPARİŞ OLUŞTURMA
            var order = new Order
            {
                UserId = userId.Value,
                CarrierCompany = carrierType
            };

            foreach (var item in cart.Items)
            {
                order.AddItem(item.Product, item.Quantity);
            }

            var customer = _context.Users.Find(userId.Value);
            order.DeliveryAddress = customer?.Address ?? "No address provided";

            // 3. LOJİSTİK VE KARGO (Factory & Decorator Pattern)
            var factory = new CarrierFactory();
            decimal weight = 5;
            decimal distance = 100;

            ICarrier carrier = factory.GetCarrier(carrierType)
                ?? throw new Exception("Carrier not found");

            if (addInsurance) // Sigorta eklemesi (Decorator)
                carrier = new InsuranceDecorator(carrier);

            if (addFragileProtection) // Hassas ürün koruması (Decorator)
                carrier = new FragileProtectionDecorator(carrier);

            order.ShippingCost = carrier.CalculateCost(weight, distance);
            order.TrackingNumber = carrier.GetTrackingNumber();
            order.CalculateTotal();

            // 4. ÖDEME SİSTEMİ (Strategy Pattern)
            IPaymentStrategy payment = paymentType switch
            {
                "creditcard" => new CreditCardPayment(
                    cardNumber ?? "",
                    cardHolder ?? "",
                    expiryDate ?? "",
                    cvv ?? ""
                ),
                _ => new BankTransferPayment()
            };

            if (!payment.ProcessPayment(order.TotalPrice))
            {
                TempData["Error"] = "Payment failed. Check your card details.";
                return RedirectToAction("Checkout");
            }

            // 5. STOK DÜŞÜRME (Observer Pattern tetiklenir)
            foreach (var item in cart.Items)
            {
                bool success = _inventoryService.TryDecreaseStock(item.Product.Id, item.Quantity);

                if (!success)
                    return Content($"Stock error while processing {item.Product.Name}");
            }

            // 6. DURUM YÖNETİMİ (State Pattern)
            order.SetState(new PendingState());
            _context.Orders.Add(order);

            var shipment = new Shipment(order, carrier);
            shipment.ProcessShipment(weight, distance);
            _context.Shipments.Add(shipment);

            // 7. SEPETİ TEMİZLE VE KAYDET
            _context.CartItems.RemoveRange(cart.Items);
            _context.SaveChanges();

            // Singleton Logger ile kayıt
            Supply_And_Logistics_System.Infrastructure.Logger.GetInstance()
                .Log($"[ORDER CREATED] #{order.Id} | User #{userId} | " +
                     $"Total: {order.TotalPrice:C} | Carrier: {carrierType} | " +
                     $"Tracking: {order.TrackingNumber}");

            return RedirectToAction("Success", new { orderId = order.Id });
        }

        // =========================
        // MY ORDERS
        // =========================
        public IActionResult MyOrders()
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
                return RedirectToAction("Login", "Account");

            var orders = _context.Orders
                .Where(o => o.UserId == userId.Value)
                .OrderByDescending(o => o.OrderDate)
                .ToList();

            return View(orders);
        }

        // =========================
        // SUCCESS
        // =========================
        public IActionResult Success(int orderId)
        {
            var order = _context.Orders
                .Include(o => o.Items)
                .ThenInclude(i => i.Product)
                .FirstOrDefault(o => o.Id == orderId);

            if (order == null)
                return NotFound();

            return View(order);
        }

        // Sipariş iptal sürecini yönetir. İade edilen stokları Composite desenine göre geri yükler.
        [HttpPost]
        public IActionResult Cancel(int id)
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            var order = _context.Orders
                .Include(o => o.Items)
                .ThenInclude(i => i.Product)
                .FirstOrDefault(o => o.Id == id && o.UserId == userId);

            if (order == null)
                return NotFound();

            if (order.StateName != "PendingState" &&
                order.StateName != "ConfirmedState" &&
                order.StateName != "InPreparationState")
            {
                TempData["Error"] = "This order cannot be cancelled.";
                return RedirectToAction("MyOrders");
            }

            order.LoadState();
            order.Cancel();

            // Stok İadesi (Composite desenine uygun olarak alt bileşenlere iade yapılır)
            foreach (var item in order.Items)
            {
                if (item.Product is CompositeProduct composite)
                {
                    _context.Entry(composite)
                        .Collection(c => c.Components)
                        .Load();

                    foreach (var component in composite.Components)
                    {
                        component.IncreaseStock(item.Quantity);

                        Supply_And_Logistics_System.Infrastructure.Logger.GetInstance()
                            .Log($"[REFUND STOCK] Component '{component.Name}' " +
                                 $"+{item.Quantity} units. New stock: {component.GetStock()}");
                    }
                }
                else
                {
                    _inventoryService.ChangeStock(item.ProductId, item.Quantity);
                }
            }

            _context.SaveChanges();

            Supply_And_Logistics_System.Infrastructure.Logger.GetInstance()
                .Log($"[REFUND] Order #{order.Id} cancelled by Customer #{userId}. " +
                     $"Amount refunded: {order.TotalPrice:C}");

            TempData["Message"] = $"Order #{order.Id} cancelled. " +
                                   $"Refund of {order.TotalPrice:C} will be processed.";

            return RedirectToAction("MyOrders");
        }

        // Bileşik ürünlerin (Composite Product) alt parçalarını veritabanından yükler.
        private void LoadCompositeComponents(Supply_And_Logistics_System.Models.Cart.Cart cart)
        {
            foreach (var item in cart.Items)
            {
                if (item.Product is CompositeProduct cp)
                {
                    _context.Entry(cp)
                        .Collection(x => x.Components)
                        .Load();
                }
            }
        }
    }
}