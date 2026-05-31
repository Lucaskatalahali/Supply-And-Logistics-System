using Supply_And_Logistics_System.Models.Orders;
using Supply_And_Logistics_System.Models.Shipping;
using Supply_And_Logistics_System.Infrastructure;

namespace Supply_And_Logistics_System.Models.Shipping
{
    /// <summary>
    /// Bir siparişe ait sevkiyat detaylarını ve lojistik sürecini yöneten sınıftır.
    /// Farklı kargo taşıyıcıları (ICarrier) ile entegre çalışır.
    /// </summary>
    public class Shipment
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public Order Order { get; set; } = null!;
        public string TrackingNumber { get; set; } = string.Empty;
        public decimal ShippingCost { get; set; }
        public string CarrierName { get; set; } = string.Empty;
        public DateTime ShippedDate { get; set; }

        // Entity Framework ve varsayılan başlatmalar için boş yapıcı metod.
        public Shipment() { }

        // Belirli bir sipariş ve taşıyıcı (carrier) ile sevkiyat nesnesi oluşturur.
        /// </summary>
        public Shipment(Order order, ICarrier carrier)
        {
            Order = order;
            OrderId = order.Id;
            ShippedDate = DateTime.UtcNow;

            // Çalışma zamanında kullanılan adaptörün sınıf adını kaydeder (Örn: ArasAdapter)
            CarrierName = carrier.GetType().Name;
            TrackingNumber = order.TrackingNumber;
            ShippingCost = order.ShippingCost;
        }

        /// Sevkiyat sürecini işler ve durumu merkezi log sistemine kaydeder.
        /// Singleton deseni (Logger) kullanılarak tüm sevkiyat kayıtları merkezi olarak tutulur.
        public void ProcessShipment(decimal weight, decimal distance)
        {
            // Singleton Logger kullanılarak operasyonel kayıt (log) oluşturulur.
            // ASP.NET'in yerleşik ILogger sınıfıyla çakışmaması için tam ad alanı (namespace) kullanılmıştır.
            Supply_And_Logistics_System.Infrastructure.Logger.GetInstance()
                .Log($"[SHIPMENT] #{OrderId} | Tracking: {TrackingNumber} | " +
                     $"Carrier: {CarrierName} | Cost: {ShippingCost:C}");
        }
    }
}