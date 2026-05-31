using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Supply_And_Logistics_System.Data;
using Supply_And_Logistics_System.Models.Identity;
using Supply_And_Logistics_System.Models.Orders.States;
using Supply_And_Logistics_System.Models.Products;

namespace Supply_And_Logistics_System.Controllers
{
    /// <summary>
    /// Depo personeli (Warehouse Staff) operasyonlarını yöneten kontrolcü.
    /// Sipariş hazırlama süreçlerini ve stok uyarılarını (Observer) takip eder.
    /// </summary>
    public class WarehouseStaffController : Controller
    {
        private readonly AppDbContext _context;

        public WarehouseStaffController(AppDbContext context)
        {
            _context = context;
        }

        // Kullanıcının Depo Personeli rolünde olup olmadığını kontrol eder.
        private bool IsWarehouseStaff()
        {
            var role = HttpContext.Session.GetString("Role");
            return role == Role.WarehouseStaff.ToString();
        }

        // Depo ana panelini görüntüler. Hazırlanması gereken (Onaylanmış, Hazırlanıyor veya İade) 
        // siparişleri listeler ve stok bildirim sayısını gösterir.
        public IActionResult Index()
        {
            if (!IsWarehouseStaff())
                return Unauthorized();

            var orders = _context.Orders
                .Where(o => o.StateName == "ConfirmedState" ||
                            o.StateName == "InPreparationState" ||
                            o.StateName == "ReturnState")
                .Include(o => o.Items)
                .ThenInclude(i => i.Product)
                .ToList();

            // Composite Pattern: Bileşik ürünlerin alt parçalarını yükler.
            foreach (var order in orders)
            {
                foreach (var item in order.Items)
                {
                    if (item.Product is CompositeProduct cp)
                    {
                        _context.Entry(cp).Collection(x => x.Components).Load();
                    }
                }
            }

            // Observer Pattern sonucu oluşan okunmamış bildirim sayısını ViewBag'e atar.
            ViewBag.NotificationCount = _context.StockNotifications.Count();

            return View(orders);
        }

        // Observer Pattern tarafından üretilen tüm düşük stok bildirimlerini listeler.
        public IActionResult Notifications()
        {
            if (!IsWarehouseStaff())
                return Unauthorized();

            var notifications = _context.StockNotifications
                .OrderByDescending(n => n.CreatedAt)
                .ToList();

            return View(notifications);
        }

        // DELETE NOTIFICATION
        [HttpPost]
        public IActionResult DeleteNotification(int id)
        {
            if (!IsWarehouseStaff())
                return Unauthorized();

            var notification = _context.StockNotifications.Find(id);

            if (notification != null)
            {
                _context.StockNotifications.Remove(notification);
                _context.SaveChanges();
            }

            return RedirectToAction("Notifications");
        }

        // DELETE ALL NOTIFICATIONS
        [HttpPost]
        public IActionResult DeleteAllNotifications()
        {
            if (!IsWarehouseStaff())
                return Unauthorized();

            var all = _context.StockNotifications.ToList();
            _context.StockNotifications.RemoveRange(all);
            _context.SaveChanges();

            return RedirectToAction("Notifications");
        }

        // Siparişi Hazırlanıyor durumuna geçirir.
        // State Pattern: Confirmed -> InPreparation geçişini sağlar.
        public IActionResult StartPreparation(int id)
        {
            if (!IsWarehouseStaff())
                return Unauthorized();

            var order = _context.Orders.FirstOrDefault(o => o.Id == id);

            if (order == null)
                return NotFound();

            order.LoadState();
            order.NextStep();

            Supply_And_Logistics_System.Infrastructure.Logger.GetInstance()
                .Log($"[WAREHOUSE] Order #{order.Id} moved to In Preparation.");

            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        // Hazırlığı biten siparişi kargoya gönderir.
        // State Pattern: InPreparation -> InTransit geçişini sağlar.
        public IActionResult SendToShipping(int id)
        {
            if (!IsWarehouseStaff())
                return Unauthorized();

            var order = _context.Orders.FirstOrDefault(o => o.Id == id);

            if (order == null)
                return NotFound();

            order.LoadState();
            order.NextStep();

            Supply_And_Logistics_System.Infrastructure.Logger.GetInstance()
                .Log($"[WAREHOUSE] Order #{order.Id} sent to shipping.");

            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        // Tüm ürünlerin güncel stok seviyelerini listeler.
        public IActionResult Stock()
        {
            if (!IsWarehouseStaff())
                return Unauthorized();

            var products = _context.Products.ToList();

            foreach (var p in products.OfType<CompositeProduct>())
            {
                _context.Entry(p).Collection(x => x.Components).Load();
            }

            return View(products);
        }

        // Mevcut bir ürüne stok eklemesi yapar.
        // Not: Bileşik ürünlere doğrudan stok eklenemez, alt bileşenlerine eklenmelidir.
        [HttpPost]
        public IActionResult AddStock(int productId, int quantity)
        {
            if (!IsWarehouseStaff())
                return Unauthorized();

            if (quantity <= 0)
            {
                TempData["Error"] = "Quantity must be greater than zero.";
                return RedirectToAction("Stock");
            }

            var product = _context.Products.Find(productId);

            if (product == null)
                return NotFound();

            if (product is CompositeProduct)
            {
                TempData["Error"] = "Cannot add stock directly to a composite product. Add stock to its components instead.";
                return RedirectToAction("Stock");
            }

            product.IncreaseStock(quantity);
            _context.SaveChanges();

            Supply_And_Logistics_System.Infrastructure.Logger.GetInstance()
                .Log($"[WAREHOUSE] Stock added: {product.Name} +{quantity} units. " +
                     $"New stock: {product.GetStock()}");

            TempData["Message"] = $"+{quantity} units added to '{product.Name}'. " +
                                   $"New stock: {product.GetStock()}";

            return RedirectToAction("Stock");
        }
    }
}