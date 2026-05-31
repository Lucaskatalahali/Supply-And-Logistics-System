using Supply_And_Logistics_System.Models.Orders.States;
using Supply_And_Logistics_System.Models.Products;
using Supply_And_Logistics_System.Models.Identity;
using System.Text.Json;

namespace Supply_And_Logistics_System.Models.Orders
{
    /// <summary>
    /// Sipariş bilgilerini yöneten merkezi sınıf.
    /// State Pattern (Durum Deseni) yapısında "Context" rolünü üstlenir.
    /// Siparişin tüm verilerini barındırır ve durum geçişlerini koordine eder.
    /// </summary>
    public class Order
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;

        /// Veritabanında saklanan durumun metinsel adı.
        public string StateName { get; set; } = "PendingState";

        // Siparişe dahil edilen ürün kalemlerinin listesi.
        public List<OrderItem> Items { get; set; } = new();

        public decimal TotalPrice { get; set; }  
        public int UserId { get; set; }
        public User User { get; set; } = null!;
        public decimal ShippingCost { get; set; }
        public string TrackingNumber { get; set; } = string.Empty;
        public string? CarrierCompany { get; set; }
        public string DeliveryAddress { get; set; } = string.Empty;

        // Mevcut durumu temsil eden nesne (Veritabanına kaydedilmez).
        // State Pattern'in çalışma zamanındaki (runtime) motorudur.
        private IOrderState? _state;

        // Siparişe yeni bir ürün ekler ve toplam fiyatı günceller.
        public void AddItem(Product product, int quantity)
        {
            Items.Add(new OrderItem
            {
                Product = product,
                Quantity = quantity,
                UnitPrice = product.GetPrice()
            });

            CalculateTotal();
        }

        // Ürünlerin toplam fiyatını ve nakliye maliyetini hesaplar.
        public void CalculateTotal()
        {
            decimal productsTotal = Items.Sum(i => i.UnitPrice * i.Quantity);
            TotalPrice = productsTotal + ShippingCost;
        }

        // Siparişin durumunu değiştirir ve veritabanı için durum adını günceller.
        public void SetState(IOrderState state)
        {
            _state = state;
            StateName = state.GetType().Name;
        }

        // Veritabanından gelen metinsel durum adını (StateName), 
        // ilgili Concrete State nesnesine dönüştürerek yükler.
        public void LoadState()
        {
            _state = StateName switch
            {
                "PendingState" => new PendingState(),
                "ConfirmedState" => new ConfirmedState(),
                "InPreparationState" => new InPreparationState(),
                "InTransitState" => new InTransitState(),
                "DeliveredState" => new DeliveredState(),
                "CancelledState" => new CancelledState(),
                "ReturnState" => new ReturnState(),
                _ => new PendingState()
            };
        }

        // --- State Pattern Delegasyon Metotları ---

        // Siparişi bir sonraki mantıksal aşamaya taşır.
        // İşlemi mevcut durum nesnesine (Concrete State) devreder.
        public void NextStep() => _state?.NextState(this);
        // Siparişi iptal eder. Karar yetkisi mevcut durumdadır.
        public void Cancel() => _state?.Cancel(this);
        // Sipariş için iade süreci başlatır.
        public void RequestReturn() => _state?.RequestReturn(this);
    }
}