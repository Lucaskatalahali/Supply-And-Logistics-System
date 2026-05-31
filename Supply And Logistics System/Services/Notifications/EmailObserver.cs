using Supply_And_Logistics_System.Models.Products;

namespace Supply_And_Logistics_System.Services.Notifications
{
    /// <summary>
    /// Stok değişikliklerini izleyen ve yöneticiye e-posta simülasyonu gönderen gözlemci sınıfı.
    /// Observer Pattern (Gözlemci Deseni) yapısında bir "Concrete Observer" birimidir.
    /// Gerçek bir SMTP sunucusu yerine, bildirimleri bir metin dosyasına yazarak simüle eder.
    /// </summary>
    public class EmailObserver : IStockObserver
    {
        // Yönetici e-posta kutusunu simüle eden dosya yolu.
        private readonly string _emailBoxPath = "Services/Notifications/AdminEmailBox.txt";

        // Stok güncellendiğinde tetiklenen metot. 
        // Kritik seviyedeki ürünler için bir e-posta formatında metni dosyaya kaydeder.
        public void UpdateStock(string productName, int currentStock)
        {
            var message =
                $"========================================\n" +
                $"[EMAIL - {DateTime.Now:dd/MM/yyyy HH:mm:ss}]\n" +
                $"To: purchasing@company.com\n" +
                $"Subject: Low Stock Alert — {productName}\n" +
                $"Body: The product '{productName}' has reached a critical stock level.\n" +
                $"Current stock: {currentStock} units. Please reorder immediately.\n" +
                $"========================================\n\n";

            // Dosyanın kaydedileceği dizinin mevcut olduğundan emin olunur.
            var dir = Path.GetDirectoryName(_emailBoxPath);
            if (!string.IsNullOrEmpty(dir))
                Directory.CreateDirectory(dir);

            // Mesaj, simüle edilen e-posta kutusu dosyasına eklenir.
            File.AppendAllText(_emailBoxPath, message);
        }
    }
}