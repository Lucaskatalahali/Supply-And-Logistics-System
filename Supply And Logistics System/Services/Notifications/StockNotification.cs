namespace Supply_And_Logistics_System.Models.Notifications
{
    /// <summary>
    /// Stok durumuyla ilgili sistem bildirimlerini temsil eden veri modeli.
    /// Observer Pattern tarafından üretilen uyarıların veritabanında saklanması için kullanılır.
    /// </summary>
    public class StockNotification
    {
        // Bildirimin benzersiz kimliği (Primary Key).
        public int Id { get; set; }

        // Bildirimin içeriği (Örn: "Ürün stoku kritik seviyenin altına düştü").
        public string Message { get; set; } = string.Empty;

        // Bildirimin oluşturulma zaman damgası.
        // Varsayılan olarak UTC formatında kayıt alınır.
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}