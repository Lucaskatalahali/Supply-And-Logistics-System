using Supply_And_Logistics_System.Models.Orders;
using Supply_And_Logistics_System.Models.Orders.States;

/// <summary>
/// Siparişin "Onaylandı" durumunu temsil eden sınıf.
/// State Pattern (Durum Deseni) yapısında bir "Concrete State" birimidir.
/// Bu aşamada sipariş geçerli kabul edilmiş ve hazırlık aşamasına geçmeye hazırdır.
/// </summary>
public class ConfirmedState : IOrderState
{
    // Siparişi bir sonraki aşama olan "Hazırlanıyor" durumuna geçirir.
    public void NextState(Order order)
    {
        // Onaylanmış sipariş depo personeli tarafından hazırlanmaya başlanır.
        order.SetState(new InPreparationState());
    }

    // Siparişi "İptal Edildi" durumuna geçirir.
    // Onaylanmış ancak henüz hazırlığı bitmemiş siparişler iptal edilebilir.
    public void Cancel(Order order)
    {
        order.SetState(new CancelledState());
    }

    // Henüz kargolanmamış veya teslim edilmemiş bir sipariş için iade talebi yapılamaz.
    public void RequestReturn(Order order)
    {
        throw new Exception("Return not allowed in Confirmed state.");
    }

    // Mevcut durumun adını döndürür.
    public string GetStatus() => "Confirmed";
}