using Supply_And_Logistics_System.Models.Orders;

namespace Supply_And_Logistics_System.Models.Orders.States
{
    /// <summary>
    /// Siparişin "Beklemede" (ilk oluşturulma) durumunu temsil eden sınıf.
    /// State Pattern (Durum Deseni) yapısında başlangıç durumunu (Initial State) temsil eder.
    /// </summary>
    public class PendingState : IOrderState
    {
        // Beklemedeki siparişi bir sonraki aşama olan "Onaylandı" durumuna geçirir.
        public void NextState(Order order)
        {
            // Ödeme kontrolü veya sistem onayı sonrası durum güncellenir.
            order.SetState(new ConfirmedState());
        }

        // Beklemedeki bir siparişi kullanıcı isteğiyle "İptal Edildi" durumuna geçirir.
        public void Cancel(Order order)
        {
            order.SetState(new CancelledState());
        }

        // Henüz onaylanmamış ve gönderilmemiş bir sipariş için iade talebi yapılamaz.
        public void RequestReturn(Order order)
        {
            throw new InvalidOperationException("Return not allowed in Pending state.");
        }

        // Mevcut durumun adını döndürür.
        public string GetStatus() => "Pending";
    }
}