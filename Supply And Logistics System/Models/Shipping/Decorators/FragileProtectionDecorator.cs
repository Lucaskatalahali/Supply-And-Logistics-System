using Supply_And_Logistics_System.Models.Shipping;

namespace Supply_And_Logistics_System.Models.Shipping.Decorators
{
    /// <summary>
    /// Kargo gönderilerine "Hassas Ürün" koruması ve ek ücreti ekleyen dekoratör sınıfı.
    /// Decorator Pattern kullanılarak mevcut taşıyıcı nesnesine dinamik olarak yeni sorumluluklar eklenir.
    /// </summary>
    public class FragileProtectionDecorator : ICarrier
    {
        // Sarmalanan (decorated) temel taşıyıcı nesnesi.
        private readonly ICarrier _innerCarrier;

        // Hassas ürün taşıma için sabit ek ücret.
        private const decimal FragileFee = 25m; // örnek

        // Mevcut bir taşıyıcıyı (Aras, Yurtici vb.) parametre olarak alıp sarmalar
        public FragileProtectionDecorator(ICarrier carrier)
        {
            _innerCarrier = carrier;
        }

        // Temel nakliye maliyetini hesaplar ve üzerine hassas koruma ücretini ekler.
        public decimal CalculateCost(decimal weight, decimal distance)
        {
            // Temel maliyeti sarmalanan nesneden alır
            var baseCost = _innerCarrier.CalculateCost(weight, distance);
            // Üzerine ek ücreti ekleyerek döndürür
            return baseCost + FragileFee;
        }

        // Takip numarası alma işlemini sarmalanan temel taşıyıcıya iletir.
        public string GetTrackingNumber()
        {
            return _innerCarrier.GetTrackingNumber();
        }
    }
}