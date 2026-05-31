namespace Supply_And_Logistics_System.Models.Identity
{
    /// <summary>
    /// Sistemdeki kullanıcı rollerini tanımlar.
    /// Rol Tabanlı Erişim Kontrolü (RBAC) için kullanılır.
    /// </summary>
    public enum Role {
        Admin, // Sistem yönetimi ve kullanıcı kontrolü
        Customer, // Sipariş veren ve takip eden son kullanıcı
        WarehouseStaff, // Stok yönetimi ve hazırlık yapan depo personeli
        Courier // Taşıma ve teslimattan sorumlu kurye
    }

    /// <summary>
    /// Kullanıcı bilgilerini ve yetkilendirme detaylarını temsil eden sınıf.
    /// </summary>
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string PasswordHash { get; set; }
        public string Email { get; set; }
        public Role Role { get; set; }
        public string? Company { get; set; } // Eğer kullanıcı kurye ise çalıştığı lojistik firmasının adı
        public string? Address { get; set; } // Teslimat veya iletişim için fiziksel adres bilgisi.

    }
}
