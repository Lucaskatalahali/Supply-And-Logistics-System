namespace Supply_And_Logistics_System.Models.Orders.States
{
    /// <summary>
    /// Sipariş durum yönetimi için temel arayüz.
    /// State Pattern (Durum Deseni) yapısındaki "State" birimidir.
    /// Farklı sipariş aşamalarının (Onaylandı, Kargoda, İptal vb.) davranışlarını standartlaştırır.
    /// </summary>
    public interface IOrderState
    {
        // Siparişi mantıksal akışa göre bir sonraki aşamaya taşır.
        void NextState(Order order);

        /// Mevcut durumun izin vermesi halinde siparişi iptal eder.
        void Cancel(Order order);

        // Sipariş için iade sürecini başlatır.
        void RequestReturn(Order order);

        // Mevcut durumun kullanıcı dostu adını döndürür
        string GetStatus();
    }
}
