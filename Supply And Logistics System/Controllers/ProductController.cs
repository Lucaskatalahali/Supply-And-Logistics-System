using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Supply_And_Logistics_System.Data;
using Supply_And_Logistics_System.Models.Cart;
using Supply_And_Logistics_System.Models.Products;

namespace Supply_And_Logistics_System.Controllers
{
    /// <summary>
    /// Ürünlerin listelenmesi ve sepete eklenmesi işlemlerini yöneten kontrolcü.
    /// Composite Pattern (Bileşik Desen) yapısındaki ürünlerin stok durumlarını kontrol eder.
    /// </summary>
    public class ProductController : Controller
    {
        private readonly AppDbContext _context;

        public ProductController(AppDbContext context)
        {
            _context = context;
        }

        // Mağaza ana sayfasında ürünleri listeler.
        // Bileşik ürünlerin (Composite) satışa uygun olup olmadığını kontrol ederek filtreler.
        public IActionResult Index()
        {
            // Tüm ürünleri veritabanından çeker.
            var products = _context.Products.ToList();

            // COMPOSITE PATTERN YÜKLEME:
            // Bileşik ürünlerin alt bileşenlerini (Components) manuel olarak belleğe yükler.
            var compositeProducts = products.OfType<CompositeProduct>().ToList();

            foreach (var cp in compositeProducts)
            {
                _context.Entry(cp)
                    .Collection(x => x.Components)
                    .Load();
            }

            // FİLTRELEME MANTIĞI:
            // Bir ürünün listede görünmesi için stokta olması gerekir.
            // Bileşik ürünler için tüm alt parçaların mevcut ve stokta olması şarttır.
            var filtered = products
                .Where(p =>
                {
                    if (p is CompositeProduct cp)
                        return cp.Components.Any() &&
                               cp.IsAvailable() &&
                               cp.GetStock() > 0;

                    return p.GetStock() > 0;
                })
                .ToList();

            return View(filtered);
        }

        // Seçilen ürünü kullanıcının sepetine ekler.
        [HttpPost]
        public IActionResult AddToCart(int productId, int quantity)
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            // Kullanıcı oturumu yoksa giriş sayfasına yönlendir.
            if (userId == null)
                return RedirectToAction("Login", "Account");

            var cart = _context.Carts
                .Include(c => c.Items)
                .FirstOrDefault(c => c.UserId == userId.Value);

            // Kullanıcının sepeti yoksa yeni bir tane oluştur.
            if (cart == null)
            {
                cart = new Cart
                {
                    UserId = userId.Value,
                    Items = new List<CartItem>()
                };

                _context.Carts.Add(cart);
                _context.SaveChanges();
            }

            // Ürün zaten sepette varsa miktarını artır, yoksa yeni kalem ekle.
            var existingItem = cart.Items
                .FirstOrDefault(i => i.ProductId == productId);

            if (existingItem != null)
            {
                existingItem.Quantity += quantity;
            }
            else
            {
                cart.Items.Add(new CartItem
                {
                    ProductId = productId,
                    Quantity = quantity,
                    CartId = cart.Id
                });
            }

            _context.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}