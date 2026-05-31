using Supply_And_Logistics_System.Models.Orders;
using Supply_And_Logistics_System.Models.Orders.States;

/// <summary>
/// Siparişin "İade Sürecinde" olduğunu temsil eden sınıf.
/// State Pattern (Durum Deseni) yapısında bir "Concrete State" birimidir.
/// Bu durum, hatalı veya hasarlı bir ürünün yeniden hazırlanmak üzere depoya geri dönmesini temsil eder.
/// </summary>
public class ReturnState : IOrderState
{
    // İade süreci tamamlandığında ve ürün depoya ulaştığında siparişi tekrar "Hazırlanıyor" durumuna geçirir.
    public void NextState(Order order)
    {
        // Depo personeli iadeyi teslim aldığında ve yeni ürün hazırlamaya başladığında durum güncellenir.
        order.SetState(new InPreparationState());
    }

    // İade sürecine girmiş bir siparişin doğrudan iptal edilmesini engeller.
    public void Cancel(Order order)
    {
        throw new InvalidOperationException("Returned order cannot be cancelled.");
    }

    /// Sipariş zaten iade sürecinde olduğu için tekrar bir iade talebi oluşturulamaz.
    public void RequestReturn(Order order)
    {
        throw new InvalidOperationException("Order already in return process.");
    }

    // Mevcut durumun adını döndürür.
    public string GetStatus() => "Return in Progress";
}