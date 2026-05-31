namespace Supply_And_Logistics_System.Models.Shipping.Adapters
{
    /// <summary>
    /// Yurtiçi Kargo harici API'sini sistemimizin standart ICarrier arayüzüne uyarlar.
    /// Adapter Pattern (Adaptör Deseni) kullanılarak farklı kargo firmaları tek bir standart üzerinden yönetilir.
    /// </summary>
    public class YurticiAdapter : ICarrier
    {
        // Yurtiçi Kargo'nun orijinal dış API referansı.
        private readonly YurticiExternalAPI _api;

        public YurticiAdapter()
        {
            _api = new YurticiExternalAPI();
        }

        // Dış API'den gelen hesaplama metodunu sistemin beklediği decimal formatına uyarlar.
        public decimal CalculateCost(decimal weight, decimal distance)
        {
            // Yurtiçi API'si double tipinde veri beklediği için casting yapılır.
            double result = _api.CalculateYurticiShippingPrice(
                (double)weight,
                (double)distance
            );

            return (decimal)result;
        }

        // Yurtiçi Kargo sisteminden kurumsal formatta bir takip numarası alır.
        public string GetTrackingNumber()
        {
            return _api.GenerateYurticiTrackingNumber();
        }
    }
}
