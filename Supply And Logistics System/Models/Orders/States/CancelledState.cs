using Supply_And_Logistics_System.Models.Orders;
using Supply_And_Logistics_System.Models.Orders.States;
/// <summary>
/// Siparişin "İptal Edildi" durumunu temsil eden sınıf.
/// State Pattern (Durum Deseni) yapısında bir "Concrete State" birimidir.
/// Bu durum nihai bir aşama olduğu için başka bir duruma geçişi engeller.
/// </summary>

public class CancelledState : IOrderState
{
    /// İptal edilmiş bir sipariş bir sonraki aşamaya geçemez.
    public void NextState(Order order)
    {
        throw new Exception("Cancelled order cannot change state.");
    }

    // Zaten iptal edilmiş bir sipariş tekrar iptal edilemez.
    public void Cancel(Order order)
    {
        throw new Exception("Order already cancelled.");
    }

    /// İptal edilmiş (hiç gönderilmemiş/tamamlanmamış) bir sipariş için iade talep edilemez.
    public void RequestReturn(Order order)
    {
        throw new Exception("Cancelled order cannot be returned.");
    }

    // Mevcut durumun adını döndürür.
    public string GetStatus() => "Cancelled";
}