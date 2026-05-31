using Supply_And_Logistics_System.Models.Products;

namespace Supply_And_Logistics_System.Models.Orders
{
    /// <summary>
    /// Siparişe ait her bir ürün kalemini temsil eden sınıf.
    /// Bir sipariş ile ürünler arasındaki ilişkiyi ve satış anındaki fiyat bilgisini tutar.
    /// </summary>
    public class OrderItem
    {
        public int Id { get; set; }

        // Bu kalemin ait olduğu siparişin benzersiz kimliği (Foreign Key).
        public int OrderId { get; set; }

        // İlgili sipariş nesnesine referans.
        public Order Order { get; set; } = null!;

        // Sipariş edilen ürün nesnesi.
        public Product Product { get; set; } = null!;

        // Sipariş edilen ürünün benzersiz kimliği (Foreign Key).
        public int ProductId { get; set; }

        // Sipariş edilen ürün miktarı.
        public int Quantity { get; set; }

        // Ürünün sipariş edildiği andaki birim fiyatı.
        public decimal UnitPrice { get; set; }
    }
}