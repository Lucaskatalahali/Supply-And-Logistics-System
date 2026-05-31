using Supply_And_Logistics_System.Models.Orders;
using Supply_And_Logistics_System.Models.Orders.States;

/// <summary>
/// Siparişin "Hazırlanıyor" durumunu temsil eden sınıf.
/// State Pattern (Durum Deseni) yapısında bir "Concrete State" birimidir.
/// Bu aşamada ürünler depoda paketlenir ve kargoya verilmeye hazır hale getirilir.
/// </summary>
public class InPreparationState : IOrderState
{
    // Hazırlığı tamamlanan siparişi "Kargoda / Taşıma Aşamasında" durumuna geçirir.
    public void NextState(Order order)
    {
        // Paketleme bittiğinde sipariş lojistik sürecine (InTransit) aktarılır.
        order.SetState(new InTransitState());
    }

    /// Hazırlık aşamasındaki bir siparişi "İptal Edildi" durumuna geçirir.
    /// Henüz kargoya verilmediği için bu aşamada iptal mümkündür.
    public void Cancel(Order order)
    {
        order.SetState(new CancelledState());
    }

    // Hazırlık aşamasındaki (henüz teslim edilmemiş) bir ürün için iade talebi yapılamaz.
    public void RequestReturn(Order order)
    {
        throw new Exception("Return not allowed in Preparation.");
    }

    // Mevcut durumun adını döndürür.
    public string GetStatus() => "In Preparation";
}