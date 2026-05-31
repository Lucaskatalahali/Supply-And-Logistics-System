namespace Supply_And_Logistics_System.Models.Products
{
    /// <summary>
    /// Stok değişikliklerini takip eden nesneler için standart arayüz.
    /// Observer Pattern yapısındaki "Observer" birimidir.
    /// </summary>
    public interface IStockObserver
    {
        // Bir ürünün stok seviyesi kritik eşiğin altına düştüğünde tetiklenen metot.
        void UpdateStock(string productName, int stock);
    }
}
