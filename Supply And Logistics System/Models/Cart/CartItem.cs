using Supply_And_Logistics_System.Models.Products;

namespace Supply_And_Logistics_System.Models.Cart;

/// <summary>
/// Alışveriş sepetindeki her bir ürün satırını temsil eden sınıf.
/// Sepet (Cart) ile Ürün (Product) arasındaki ilişkiyi ve seçilen miktarı yönetir.
/// </summary>
public class CartItem
{
    // Sepet kaleminin benzersiz kimliği (Primary Key).
    public int Id { get; set; }

    // Bu kalemin ait olduğu sepetin kimliği (Foreign Key).
    public int CartId { get; set; }

    // İlgili sepet nesnesine referans.
    public Cart Cart { get; set; }

    // Sepete eklenen ürünün kimliği (Foreign Key).
    public int ProductId { get; set; }

    // Sepete eklenen ürün nesnesi.
    public Product Product { get; set; }

    // Kullanıcının bu üründen sepetine kaç adet eklediğini belirten miktar.
    public int Quantity { get; set; }
}
