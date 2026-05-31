using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Supply_And_Logistics_System.Data;
using Supply_And_Logistics_System.Models.Identity;

namespace Supply_And_Logistics_System.Controllers
{
    /// <summary>
    /// Kurye operasyonlarını yöneten kontrolcü.
    /// Siparişlerin teslimat ve iade süreçlerini State Pattern kullanarak yönetir.
    /// </summary>
    public class CourierController : Controller
    {
        private readonly AppDbContext _context;

        public CourierController(AppDbContext context)
        {
            _context = context;
        }

        // Mevcut kullanıcının Kurye rolünde olup olmadığını kontrol eder.
        private bool IsCourier()
        {
            var role = HttpContext.Session.GetString("Role");
            return role == Role.Courier.ToString();
        }

        // Kuryenin bağlı olduğu şirkete ait ve taşıma aşamasındaki (InTransit) siparişleri listeler.
        public IActionResult Index()
        {
            if (!IsCourier())
                return Unauthorized();

            var userId = HttpContext.Session.GetInt32("UserId");
            var courier = _context.Users.FirstOrDefault(u => u.Id == userId);

            if (courier == null)
                return RedirectToAction("Login", "Account");

            // Sadece bu kuryenin şirketine atanmış ve yolda olan siparişleri getirir.
            var orders = _context.Orders
                .Where(o => o.StateName == "InTransitState"
                         && o.CarrierCompany == courier.Company)
                .Include(o => o.Items)
                .ThenInclude(i => i.Product)
                .ToList();

            return View(orders);
        }


        // Siparişi "Teslim Edildi" olarak işaretler.
        // State Pattern: InTransit -> Delivered geçişini tetikler.
        [HttpPost]
        public IActionResult Deliver(int orderId)
        {
            if (!IsCourier())
                return Unauthorized();

            var userId = HttpContext.Session.GetInt32("UserId");
            var courier = _context.Users.FirstOrDefault(u => u.Id == userId);

            if (courier == null)
                return Unauthorized();

            var order = _context.Orders
                .FirstOrDefault(o => o.Id == orderId &&
                                     o.CarrierCompany == courier.Company);

            if (order == null)
                return NotFound();

            // Durum deseni yüklenir ve bir sonraki aşamaya (Delivered) geçilir.
            order.LoadState();
            order.NextStep(); // InTransit -> Delivered

            // Singleton Logger ile teslimat kaydı tutulur.
            Supply_And_Logistics_System.Infrastructure.Logger.GetInstance()
                .Log($"[DELIVERED] Order #{order.Id} delivered by courier {courier.Name}.");

            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        // Teslimat sırasında bir sorun çıkarsa (örneğin kusurlu ürün) iade sürecini başlatır.
        // State Pattern: InTransit -> ReturnState geçişini tetikler.
        [HttpPost]
        public IActionResult RequestReturn(int orderId)
        {
            if (!IsCourier())
                return Unauthorized();

            var userId = HttpContext.Session.GetInt32("UserId");
            var courier = _context.Users.FirstOrDefault(u => u.Id == userId);

            if (courier == null)
                return Unauthorized();

            var order = _context.Orders
                .FirstOrDefault(o => o.Id == orderId &&
                                     o.CarrierCompany == courier.Company);

            if (order == null)
                return NotFound();

            // Durum deseni yüklenir ve iade süreci tetiklenir.
            order.LoadState();
            order.RequestReturn(); // InTransit -> ReturnState

            Supply_And_Logistics_System.Infrastructure.Logger.GetInstance()
                .Log($"[RETURN] Order #{order.Id} returned by courier {courier.Name}. Defective product.");

            _context.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}