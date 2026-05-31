using Supply_And_Logistics_System.Data;
using Supply_And_Logistics_System.Models.Products;
using Supply_And_Logistics_System.Infrastructure;
using Supply_And_Logistics_System.Services.Notifications;

namespace Supply_And_Logistics_System.Services
{
    /// <summary>
    /// Ürün stok seviyelerini ve envanter hareketlerini yöneten servis sınıfı.
    /// Stok değişikliklerini izlemek için Observer Pattern (Gözlemci Deseni) kullanır.
    /// </summary>
    public class InventoryService
    {
        private readonly AppDbContext _context;

        public InventoryService(AppDbContext context)
        {
            _context = context;
        }

        // Belirli bir miktarda stok azaltmaya çalışır. 
        // Stok yeterliyse işlemi gerçekleştirir ve (observers) tetikler.
        public bool TryDecreaseStock(int productId, int quantity)
        {
            var product = _context.Products.Find(productId);

            if (product == null)
                return false;

            // Stok kontrolü
            if (product.GetStock() < quantity)
            {
                Logger.GetInstance()
                    .Log($"[STOCK ERROR] Not enough stock for {product.Name}");
                return false;
            }

            // OBSERVER PATTERN (Gözlemci Deseni)
            // Stok azaldığında bilgilendirme yapacak olan gözlemciler sisteme kaydedilir.
            product.AddObserver(new EmailObserver());
            product.AddObserver(new InternalNotificationObserver(_context));

            // Stok düşürme işlemi (Bu işlem içeride NotifyObservers metodunu tetikler)
            product.DecreaseStock(quantity);

            // Singleton Logger ile işlem kaydı
            Logger.GetInstance()
                .Log($"[STOCK] Decreased stock for: {product.Name}. " +
                     $"Remaining: {product.GetStock()}");

            _context.SaveChanges();

            return true;
        }

        // Stok miktarını manuel olarak artırır veya azaltır.
        public void ChangeStock(int productId, int quantityChange)
        {
            var product = _context.Products.Find(productId);

            if (product == null)
                return;

            if (quantityChange < 0)
            {
                // Azaltma işlemi için gözlemcileri burada da kaydet
                product.AddObserver(new EmailObserver());
                product.AddObserver(new InternalNotificationObserver(_context));

                product.DecreaseStock(Math.Abs(quantityChange));

                Logger.GetInstance()
                    .Log($"[STOCK] Decreased: {product.Name}");
            }
            else
            {
                // Stok artırımı (Gözlemci tetiklenmesine gerek duyulmayan basit işlem)
                product.IncreaseStock(quantityChange);

                Logger.GetInstance()
                    .Log($"[STOCK] Increased: {product.Name}");
            }

            _context.SaveChanges();
        }
    }
}