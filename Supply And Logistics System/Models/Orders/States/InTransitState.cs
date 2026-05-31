using Supply_And_Logistics_System.Models.Orders;
using Supply_And_Logistics_System.Models.Orders.States;

/// <summary>
/// Siparişin "Taşıma Aşamasında / Kargoda" olduğunu temsil eden sınıf.
/// State Pattern (Durum Deseni) yapısında bir "Concrete State" birimidir.
/// Bu aşama lojistik sürecini ve kurye etkileşimini temsil eder.
/// </summary>
public class InTransitState : IOrderState
{
    // Kurye "Teslim Edildi" olarak işaretlediğinde siparişi bir sonraki aşamaya geçirir.
    public void NextState(Order order)
    {
        // Kurye teslimatın tamamlandığını onayladığında durum "Teslim Edildi"ye geçer ("Mark as Delivered")
        order.SetState(new DeliveredState());
    }

    // Taşıma aşamasındaki bir siparişin iptal edilmesini engeller.
    public void Cancel(Order order)
    {
        // Ürün yola çıktığı için standart iptal süreci artık kapalıdır.
        throw new InvalidOperationException("Cannot cancel an order that is already in transit.");
    }

    // Kurye teslimat sırasında bir sorun (hasarlı ürün vb.) fark ederse iade sürecini başlatır.
    public void RequestReturn(Order order)
    {
        // Ürün hatalıysa kurye tarafından doğrudan iade durumuna (ReturnState) çekilebilir.
        order.SetState(new ReturnState());
    }

    // Mevcut durumun adını döndürür.
    public string GetStatus() => "In Transit";
}