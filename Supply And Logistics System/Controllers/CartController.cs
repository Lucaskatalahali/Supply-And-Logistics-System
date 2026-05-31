using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Supply_And_Logistics_System.Data;
using Supply_And_Logistics_System.Models.Cart;
using Supply_And_Logistics_System.Models.Products;

namespace Supply_And_Logistics_System.Controllers
{
    // <summary>
    // Kullanıcının alışveriş sepeti işlemlerini (listeleme, güncelleme, silme) yöneten kontrolcü.
    // Composite Pattern yapısındaki ürünlerin sepet içinde doğru gösterilmesini sağlar.
    // </summary>
    public class CartController : Controller
    {
        private readonly AppDbContext _context;

        public CartController(AppDbContext context)
        {
            _context = context;
        }

        // Giriş yapmış kullanıcının sepet içeriğini görüntüler.
        public IActionResult Index()
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            // Kullanıcı oturumu kapalıysa giriş sayfasına yönlendir.
            if (userId == null)
                return RedirectToAction("Login", "Account");

            var cart = _context.Carts
                .Include(c => c.Items)
                .ThenInclude(i => i.Product)
                .FirstOrDefault(c => c.UserId == userId.Value);

            if (cart == null)
                return View(new Cart { Items = new List<CartItem>() });

            // COMPOSITE PATTERN YÜKLEME:
            // Eğer sepetteki ürün bir "CompositeProduct" (Bileşik Ürün) ise,
            // bu ürünün alt bileşenlerini de veritabanından yükle.
            foreach (var item in cart.Items)
            {
                if (item.Product is CompositeProduct cp)
                {
                    _context.Entry(cp)
                        .Collection(x => x.Components)
                        .Load();
                }
            }

            return View(cart);
        }

        // Belirli bir ürünü sepetten tamamen kaldırır.
        [HttpPost]
        public IActionResult Remove(int productId)
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
                return RedirectToAction("Login", "Account");

            var cart = _context.Carts
                .Include(c => c.Items)
                .FirstOrDefault(c => c.UserId == userId.Value);

            if (cart == null)
                return RedirectToAction("Index");

            var item = cart.Items.FirstOrDefault(x => x.ProductId == productId);

            if (item != null)
            {
                _context.CartItems.Remove(item);
                _context.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        // Sepetteki bir ürünün miktarını (adet) günceller.
        [HttpPost]
        public IActionResult Update(int productId, int quantity)
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
                return RedirectToAction("Login", "Account");

            var cart = _context.Carts
                .Include(c => c.Items)
                .FirstOrDefault(c => c.UserId == userId.Value);

            if (cart == null)
                return RedirectToAction("Index");

            var item = cart.Items.FirstOrDefault(x => x.ProductId == productId);

            if (item != null)
            {
                item.Quantity = quantity;
                _context.SaveChanges();
            }

            return RedirectToAction("Index");
        }
    }
}