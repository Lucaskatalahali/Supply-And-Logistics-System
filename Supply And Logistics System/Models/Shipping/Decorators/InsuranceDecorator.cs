namespace Supply_And_Logistics_System.Models.Shipping.Decorators
{
    /// <summary>
    /// Kargo gönderilerine sigorta hizmeti ve ek ücreti ekleyen dekoratör sınıfı.
    /// Decorator Pattern (Dekoratör Deseni) kullanılarak temel nakliye hizmetine 
    /// dinamik olarak sigorta maliyeti eklenir.
    /// </summary>
    public class InsuranceDecorator : ICarrier
    {
        // Sarmalanan (decorated) temel taşıyıcı nesnesi referansı.
        private readonly ICarrier _innerCarrier;

        // Kargo sigortası için belirlenen sabit ek ücret.
        private const decimal InsuranceFee = 50m;

        // Mevcut bir taşıyıcıyı sarmalayarak sigorta özelliği kazandırır.
        public InsuranceDecorator(ICarrier carrier)
        {
            _innerCarrier = carrier;
        }

        // Alt taşıyıcıdan gelen temel maliyeti alır ve sigorta ücretini ekler.
        public decimal CalculateCost(decimal weight, decimal distance)
        {
            // Temel taşıma maliyetini hesaplatır
            decimal baseCost = _innerCarrier.CalculateCost(weight, distance);

            // Sigorta bedelini ekleyerek toplam tutarı döndürür
            return baseCost + InsuranceFee;
        }

        // Takip numarası üretimini sarmalanan orijinal taşıyıcıya iletir.
        public string GetTrackingNumber()
        {
            //Orijinal kargo firmasının takip numarasını korur
            return _innerCarrier.GetTrackingNumber();
        }
    }
}
