namespace Supply_And_Logistics_System.Models.Shipping.Adapters
{
    /// <summary>
    /// Aras Kargo harici API'sini sistemimizin standart ICarrier arayüzüne uyarlar.
    /// Adapter Pattern (Adaptör Deseni) kullanılarak dış servis bağımlılığı izole edilmiştir.
    /// </summary>
    public class ArasAdapter : ICarrier
    {
        // Aras Kargo'nun orijinal dış API referansı.
        private readonly ArasExternalAPI _api;

        public ArasAdapter()
        {
            _api = new ArasExternalAPI();
        }

        // Dış API'den gelen hesaplama metodunu sistemin decimal formatına dönüştürür.
        public decimal CalculateCost(decimal weight, decimal distance)
        {
            // Aras API'si double tipinde veri beklediği için tür dönüşümü (casting) yapılır.
            double result = _api.CalculateArasShippingPrice(
                (double)weight,
                (double)distance
            );

            return (decimal)result;
        }

        // Aras Kargo sisteminden benzersiz bir takip numarası alır.
        public string GetTrackingNumber()
        {
            return _api.GenerateArasTrackingNumber();
        }
    }
}