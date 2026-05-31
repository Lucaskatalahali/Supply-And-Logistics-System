using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Supply_And_Logistics_System.Data;
using Supply_And_Logistics_System.Models.Identity;
using Supply_And_Logistics_System.Models.Products;

namespace Supply_And_Logistics_System.Controllers
{
    /// <summary>
    /// Sistem yöneticisi (Admin) yetkilerine sahip kullanıcılar için operasyonel kontrolcü.
    /// Ürün yönetimi, personel kaydı ve sipariş onaylama süreçlerini yönetir.
    /// </summary>
    public class AdminController : Controller
    {
        private readonly AppDbContext _context;

        public AdminController(AppDbContext context)
        {
            _context = context;
        }

        // Mevcut kullanıcının Admin rolüne sahip olup olmadığını kontrol eden yardımcı metod.
        private bool IsAdmin()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null) return false;
            var user = _context.Users.Find(userId.Value);
            return user != null && user.Role == Role.Admin;
        }

        // Tüm ürünlerin listelendiği ana sayfa.
        // Composite Pattern yapısındaki bileşenleri (Components) dahil ederek getirir.
        public IActionResult Index()
        {
            if (!IsAdmin()) return Content("Access denied.");

            var products = _context.Products
                .Include("Components")
                .ToList();

            return View(products);
        }

        // Yeni ürün oluşturma sayfasını görüntüler. (GET)
        [HttpGet]
        public IActionResult CreateProduct()
        {
            if (!IsAdmin()) return Content("Access denied.");

            // Kompozit ürün oluştururken seçilebilecek basit ürünleri listeler.
            ViewBag.Products = _context.Products
                .Where(p => p is SimpleProduct)
                .ToList();

            return View();
        }

        // Yeni bir basit veya kompozit ürün oluşturur. (POST)
        [HttpPost]
        public IActionResult CreateProduct(
            string productType,
            string name,
            string description,
            decimal price,
            int stock,
            int threshold,
            int[]? components)
        {
            if (!IsAdmin()) return Content("Access denied.");

            if (productType == "simple")
            {
                var product = new SimpleProduct
                {
                    Name = name,
                    Description = description,
                    Price = price,
                    Stock = stock,
                    Threshold = threshold
                };
                _context.Products.Add(product);
            }
            else
            {
                // Composite Pattern Uygulaması
                if (components == null || components.Length == 0)
                    return Content("Composite product must have components.");

                var composite = new CompositeProduct
                {
                    Name = name,
                    Threshold = threshold
                };

                var selectedProducts = _context.Products
                    .Where(p => components.Contains(p.Id) && p is SimpleProduct)
                    .ToList();

                if (!selectedProducts.Any())
                    return Content("Invalid components.");

                foreach (var p in selectedProducts)
                    composite.AddComponent(p);

                _context.Products.Add(composite);
            }

            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        // =========================
        // GET EDIT PRODUCT
        // =========================
        [HttpGet]
        public IActionResult EditProduct(int id)
        {
            if (!IsAdmin()) return Content("Access denied.");

            var product = _context.Products.Find(id);
            if (product == null) return NotFound();

            return View(product);
        }

        // =========================
        // POST EDIT PRODUCT
        // =========================
        [HttpPost]
        public IActionResult EditProduct(
            int id,
            string name,
            string description,
            decimal price,
            int stock,
            int threshold)
        {
            if (!IsAdmin()) return Content("Access denied.");

            var product = _context.Products.Find(id);
            if (product == null) return NotFound();

            product.Name = name;

            if (product is SimpleProduct simple)
            {
                simple.Description = description;
                simple.Price = price;
                simple.Stock = stock;
                simple.Threshold = threshold;
            }
            else if (product is CompositeProduct composite)
            {
                composite.Threshold = threshold;
            }

            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        // =========================
        // DELETE PRODUCT
        // =========================
        [HttpPost]
        public IActionResult DeleteProduct(int id)
        {
            if (!IsAdmin()) return Content("Access denied.");

            var product = _context.Products.Find(id);
            if (product == null) return NotFound();

            _context.Products.Remove(product);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        // Bekleyen (Pending) durumundaki siparişleri listeler.
        public IActionResult Orders()
        {
            if (!IsAdmin()) return Content("Access denied.");

            var orders = _context.Orders
                .Include(o => o.Items)
                .ThenInclude(i => i.Product)
                .Where(o => o.StateName == "PendingState")
                .ToList();

            return View(orders);
        }

        // State Pattern kullanarak bir siparişi onaylar ve bir sonraki duruma geçirir.
        [HttpPost]
        public IActionResult ApproveOrder(int orderId)
        {
            if (!IsAdmin()) return Content("Access denied.");

            var order = _context.Orders.FirstOrDefault(o => o.Id == orderId);
            if (order == null) return NotFound();

            // State Pattern tetiklenir: Beklemede -> Onaylandı
            order.LoadState();
            order.NextStep();

            _context.SaveChanges();
            return RedirectToAction("Orders");
        }

        [HttpGet]
        public IActionResult RegisterWorker()
        {
            if (!IsAdmin()) return Content("Access denied.");
            return View();
        }

        // Yeni bir çalışan (Admin, Depo Görevlisi veya Kurye) kaydeder.
        [HttpPost]
        public IActionResult RegisterWorker(
            string name,
            string email,
            string password,
            int role,
            string? company)
        {
            if (!IsAdmin()) return Content("Access denied.");

            if (role != 0 && role != 2 && role != 3)
                return Content("Invalid role.");

            var existingUser = _context.Users.FirstOrDefault(u => u.Email == email);
            if (existingUser != null)
                return Content("This email is already registered.");

            if (role == 3 && string.IsNullOrEmpty(company))
                return Content("Courier must have a company.");

            var user = new User
            {
                Name = name,
                Email = email,
                PasswordHash = password,
                Role = (Role)role,
                Company = role == 3 ? company : null
            };

            _context.Users.Add(user);
            _context.SaveChanges();

            // Singleton Logger ile işlem kaydı tutulur.
            Infrastructure.Logger.GetInstance()
                .Log($"[ADMIN] New user registered: {name} | Role: {(Role)role} | Email: {email}");

            return RedirectToAction("Workers");
        }

        // =========================
        // WORKERS LIST
        // =========================
        public IActionResult Workers()
        {
            if (!IsAdmin()) return Content("Access denied.");

            var workers = _context.Users
                .Where(u => u.Role == Role.Admin ||
                            u.Role == Role.WarehouseStaff ||
                            u.Role == Role.Courier)
                .ToList();

            return View(workers);
        }

        // Bir personeli sistemden siler (Kendi hesabı veya siparişi olanlar hariç).
        [HttpPost]
        public IActionResult DeleteWorker(int id)
        {
            if (!IsAdmin()) return Content("Access denied.");

            var currentUserId = HttpContext.Session.GetInt32("UserId");

            // Admin kendi silemez
            if (id == currentUserId)
            {
                TempData["Error"] = "You cannot delete your own account.";
                return RedirectToAction("Workers");
            }

            var user = _context.Users.Find(id);
            if (user == null) return NotFound();

            // (Buradan) customer silemez
            if (user.Role == Role.Customer)
            {
                TempData["Error"] = "Cannot delete a customer from here.";
                return RedirectToAction("Workers");
            }

            // sipareş varsa silemez
            var hasOrders = _context.Orders.Any(o => o.UserId == id);
            if (hasOrders)
            {
                TempData["Error"] = $"Cannot delete '{user.Name}' — they have orders in the system.";
                return RedirectToAction("Workers");
            }

            _context.Users.Remove(user);
            _context.SaveChanges();

            Infrastructure.Logger.GetInstance()
                .Log($"[ADMIN] User deleted: {user.Name} | Role: {user.Role}");

            TempData["Message"] = $"User '{user.Name}' deleted successfully.";
            return RedirectToAction("Workers");
        }
    }
}