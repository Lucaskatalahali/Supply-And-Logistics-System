namespace Supply_And_Logistics_System.Models.Shipping.Adapters
{
    /// <summary>
    /// Aras Kargo'nun gerçek dış API servisini simüle eden sınıftır.
    /// Bu sınıf sistemimizden bağımsız bir yapıdadır ve metod isimleri dış kurallara bağlıdır.
    /// </summary>
    public class ArasExternalAPI
    {
        // Aras Kargo'ya özel algoritmalarla nakliye ücretini hesaplar.
        public double CalculateArasShippingPrice(double weight, double distance)
        {
            // Basit fiyat hesaplama simülasyonu
            return (weight * 8.5) + (distance * 0.4);
        }

        // Aras Kargo standartlarına uygun benzersiz bir takip numarası oluşturur.
        public string GenerateArasTrackingNumber()
        {
            return "ARAS-" + Guid.NewGuid().ToString().Substring(0, 8).ToUpper();
        }
    }
}
