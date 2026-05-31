using Supply_And_Logistics_System.Models.Identity;

namespace Supply_And_Logistics_System.Models.Cart
{
    // <summary>
    // Kullanıcının alışveriş sepetini temsil eden sınıf.
    // Sipariş oluşturulmadan önce seçilen ürünlerin geçici olarak tutulduğu yerdir.
    // </summary>
    public class Cart
    {
        // Sepetin benzersiz kimliği (Primary Key).
        public int Id { get; set; }

        // Bu sepetin ait olduğu kullanıcının kimliği (Foreign Key).
        public int UserId { get; set; }

        // Sepet sahibi kullanıcı nesnesine referans.
        public User User { get; set; }

        // Sepete eklenen ürün kalemlerinin (CartItem) listesi.
        // Kullanıcı sepetine birden fazla ürün ekleyebilir.
        public List<CartItem> Items { get; set; } = new();
    }
}
