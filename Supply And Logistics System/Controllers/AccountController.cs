using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Supply_And_Logistics_System.Data;
using Supply_And_Logistics_System.Models.Cart;
using Supply_And_Logistics_System.Models.Identity;
using System.Linq;

namespace Supply_And_Logistics_System.Controllers
{
    // <summary>
    // Kullanıcı kayıt, giriş ve oturum yönetimi işlemlerinden sorumlu kontrolcü.
    // Sistem erişim yetkilerini ve kullanıcı oturum verilerini (Session) yönetir.
    // </summary>
    public class AccountController : Controller
    {
        private readonly AppDbContext _context;

        public AccountController(AppDbContext context)
        {
            _context = context;
        }

        // Kayıt olma sayfasını görüntüler. (GET)
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        // Yeni kullanıcı kaydı oluşturur ve kullanıcıya otomatik olarak boş bir sepet atar. (POST)
        [HttpPost]
        public IActionResult Register(string name, string email, string password, string address)
        {
            // E-postanın sistemde kayıtlı olup olmadığını kontrol eder.
            var existingUser = _context.Users.FirstOrDefault(u => u.Email == email);

            if (existingUser != null)
                return Content("This email is already registered.");

            // Yeni kullanıcı nesnesi oluşturma
            var user = new User
            {
                Name = name,
                Email = email,
                PasswordHash = password,
                Role = Role.Customer,
                Address = address
            };

            _context.Users.Add(user);
            _context.SaveChanges();

            // Her yeni kullanıcı için otomatik olarak bir sepet (Cart) oluşturulur.
            var cart = new Cart { UserId = user.Id };
            _context.Carts.Add(cart);
            _context.SaveChanges();

            return RedirectToAction("Login");
        }

        // Giriş yapma sayfasını görüntüler. (GET)
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // Kullanıcı kimlik bilgilerini doğrular ve oturum (Session) başlatır. (POST)
        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == email);

            if (user == null)
                return Content("User not found.");

            if (user.PasswordHash != password)
                return Content("Wrong password.");

            // OTURUM YÖNETİMİ (Session)
            // Kullanıcı ID ve rol bilgileri oturum süresince saklanır.
            HttpContext.Session.SetInt32("UserId", user.Id);
            HttpContext.Session.SetString("Role", user.Role.ToString());

            return RedirectToAction("Index", "Home");                                     
        }

        // Kullanıcı oturumunu sonlandırır ve giriş sayfasına yönlendirir.
        public IActionResult Logout()
        {
            HttpContext.Session.Clear(); // Tüm oturum verilerini siler.
            return RedirectToAction("Login");
        }
    }
}