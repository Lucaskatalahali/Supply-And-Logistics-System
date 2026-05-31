namespace Supply_And_Logistics_System.Models.Shipping.Adapters
{
    /// <summary>
    /// GlobalExpress harici API'sini sistemimizin standart ICarrier arayüzüne uyarlar.
    /// Adapter Pattern (Adaptör Deseni) sayesinde sistem farklı lojistik protokollerini destekler.
    /// </summary>
    public class GlobalExpressAdapter : ICarrier
    {
        // GlobalExpress firmasına ait dış API referansı.
        private readonly GlobalExpressExternalAPI _api;

        public GlobalExpressAdapter()
        {
            _api = new GlobalExpressExternalAPI();
        }

        // Küresel kargo standartlarına göre maliyeti hesaplar ve decimal tipine dönüştürür.
        public decimal CalculateCost(decimal weight, decimal distance)
        {
            // Dış API double beklediği için uygun tip dönüşümü yapılır.
            double result = _api.CalculateGlobalExpressShippingPrice(
                (double)weight,
                (double)distance
            );

            return (decimal)result;
        }

        // GlobalExpress sisteminden uluslararası formatta bir takip numarası alır.
        public string GetTrackingNumber()
        {
            return _api.GenerateGlobalExpressTrackingNumber();
        }
    }
}
