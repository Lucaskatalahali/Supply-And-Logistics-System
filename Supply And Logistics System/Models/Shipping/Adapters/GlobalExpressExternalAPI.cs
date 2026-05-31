namespace Supply_And_Logistics_System.Models.Shipping.Adapters
{
    /// <summary>
    /// GlobalExpress firmasının gerçek dış API servisini simüle eden sınıftır.
    /// Bu sınıf sistemimizden bağımsızdır ve kendine has isimlendirme kurallarına sahiptir.
    /// </summary>
    public class GlobalExpressExternalAPI
    {
        // GlobalExpress'e özel uluslararası tarifelerle kargo ücretini hesaplar.
        public double CalculateGlobalExpressShippingPrice(double weight, double distance)
        {
            // Basit uluslararası kargo fiyatlandırma simülasyonu
            return (weight * 10.0) + (distance * 0.6);
        }

        // GlobalExpress standartlarında benzersiz bir uluslararası takip numarası oluşturur.
        public string GenerateGlobalExpressTrackingNumber()
        {
            return "GLOBAL-" + Guid.NewGuid().ToString().Substring(0, 8).ToUpper();
        }
    }
}
