using Supply_And_Logistics_System.Data;
using Supply_And_Logistics_System.Models.Notifications;
using Supply_And_Logistics_System.Models.Products;

namespace Supply_And_Logistics_System.Services.Notifications
{
    /// <summary>
    /// Stok değişikliklerini izleyen ve sistem içine bildirim kaydeden somut gözlemci sınıfı.
    /// Observer Pattern yapısında bir "Concrete Observer" birimidir.
    /// </summary>
    public class InternalNotificationObserver : IStockObserver
    {
        private readonly AppDbContext _context;

        // Veritabanı bağlamını (context) alarak bildirimleri kaydetmek üzere yapılandırılır.
        public InternalNotificationObserver(AppDbContext context)
        {
            _context = context;
        }

        // Stok güncellendiğinde tetiklenen metot. 
        // Kritik stok durumunda veritabanına yeni bir StockNotification ekler.
        public void UpdateStock(string productName, int currentStock)
        {
            // Yeni bir sistem bildirimi nesnesi oluşturulur.
            var notification = new StockNotification
            {
                Message = $"⚠️ Stock low for '{productName}' — {currentStock} units remaining. Action required.",
                CreatedAt = DateTime.UtcNow
            };

            // Bildirim veritabanına kaydedilir.
            _context.StockNotifications.Add(notification);
            _context.SaveChanges();
        }
    }
}