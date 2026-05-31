using Supply_And_Logistics_System.Models.Shipping.Adapters;

namespace Supply_And_Logistics_System.Models.Shipping
{
    /// <summary>
    /// Doğru lojistik taşıyıcısını (carrier) oluşturmaktan sorumlu olan fabrika sınıfı.
    /// Factory Method Pattern (Fabrika Metodu Deseni) kullanılarak nesne oluşturma mantığı kapsüllenmiştir.
    /// </summary>
    public class CarrierFactory
    {
        // Verilen tür ismine göre uygun taşıyıcı adaptörünü (Aras, Yurtici, GlobalExpress) döndürür.
        public ICarrier GetCarrier(string type)
        {
            // Kullanıcının seçimine göre ilgili adaptör sınıfını başlatır.
            // Bu sayede istemci kod, hangi sınıfın (Adapter) örnekleneceğini bilmek zorunda kalmaz.
            return type.ToLower() switch
            {
                "aras" => new ArasAdapter(),
                "yurtici" => new YurticiAdapter(),
                "globalexpress" => new GlobalExpressAdapter(),

                // Tanımsız bir kargo firması seçilirse sistem hata döner.
                _ => throw new Exception("Invalid carrier type.")
            };
        }
    }
}