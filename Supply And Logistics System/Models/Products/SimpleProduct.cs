namespace Supply_And_Logistics_System.Models.Products
{
    /// <summary>
    /// Tekil bir ürünü temsil eden sınıf. 
    /// Composite Pattern yapısında Yaprak birimini temsil eder.
    /// </summary>
    public class SimpleProduct : Product
    {
        //Veritabanında saklanan ham fiyat değerini döndürür.
        public override decimal GetPrice() => Price; 

        //Veritabanında saklanan mevcut stok miktarını döndürür.
        public override int GetStock() => Stock;
    }
}
