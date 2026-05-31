namespace Supply_And_Logistics_System.Models.Shipping.Adapters
{
    /// <summary>
    /// Yurtiçi Kargo'nun gerçek dış API servisini simüle eden sınıftır.
    /// Bu sınıf sistemimizden bağımsız bir yapıdadır ve metod isimleri kargo firmasının kendi standartlarına göredir.
    /// </summary>
    public class YurticiExternalAPI
    {
        public double CalculateYurticiShippingPrice(double weight, double distance)
        {
            // Basit bir fiyat hesaplama simülasyonu
            return (weight * 7.0) + (distance * 0.5);
        }

        // Yurtiçi Kargo standartlarına uygun benzersiz bir takip numarası oluşturur.
        public string GenerateYurticiTrackingNumber()
        {
            return "YURTICI-" + Guid.NewGuid().ToString().Substring(0, 8).ToUpper();
        }
    }
}
