using Supply_And_Logistics_System.Models.Orders;
using Supply_And_Logistics_System.Models.Orders.States;

/// <summary>
/// Siparişin "Teslim Edildi" durumunu temsil eden sınıf.
/// State Pattern (Durum Deseni) yapısında bir "Concrete State" birimidir.
/// Bu durum, sipariş döngüsünün başarıyla tamamlandığını gösterir.
/// </summary>
public class DeliveredState : IOrderState
{
    // Sipariş zaten teslim edildiği için bir sonraki ana duruma geçiş yapılamaz.
    // Akış burada sonlanır.
    public void NextState(Order order)
    {
        throw new InvalidOperationException("Order already delivered. Flow is complete.");
    }

    // Teslim edilmiş bir siparişin iptal edilmesi mümkün değildir.
    public void Cancel(Order order)
    {
        throw new InvalidOperationException("Cannot cancel a delivered order.");
    }

    // Teslimat sonrası işlemler farklı bir süreç (iade modülü gibi) gerektirir; 
    // bu basit akışta teslim edilmiş sipariş için yeni bir talep doğrudan bu metodla oluşturulmaz.
    public void RequestReturn(Order order)
    {
        throw new InvalidOperationException("Order already delivered. Contact support.");
    }

    // Mevcut durumun adını döndürür.
    public string GetStatus() => "Delivered";
}