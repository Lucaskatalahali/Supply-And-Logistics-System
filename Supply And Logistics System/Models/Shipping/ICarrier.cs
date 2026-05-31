namespace Supply_And_Logistics_System.Models.Shipping
{
    /// <summary>
    /// Tüm lojistik taşıyıcıları için standart sözleşmeyi tanımlayan arayüz.
    /// Bu arayüz; Adapter, Decorator ve Factory Method desenlerinin temel yapısını oluşturur.
    /// </summary>
    /// 
    public interface ICarrier
    {
        /// Paketin ağırlığı ve gidilecek mesafeye göre nakliye maliyetini hesaplar.
        decimal CalculateCost(decimal weight, decimal distance);
        // İlgili kargo firmasından benzersiz bir takip numarası alır.
        string GetTrackingNumber();
    }
}
