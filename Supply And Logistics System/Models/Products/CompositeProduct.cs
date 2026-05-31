using Supply_And_Logistics_System.Models.Products;

namespace Supply_And_Logistics_System.Models.Products
{
    /// <summary>
    /// Birden fazla üründen oluşan paket (bundle) ürünleri temsil eder.
    /// Composite Pattern (Bileşik Desen) yapısında "Composite" birimidir.
    /// Fiyat ve stok gibi verileri alt bileşenlerinden dinamik olarak hesaplar.
    /// </summary>
    public class CompositeProduct : Product
    {
        /// Paketi oluşturan alt ürünlerin listesi.
        /// Entity Framework gereksinimi nedeniyle public olarak tanımlanmıştır.
        public List<Product> Components { get; set; } = new();

        // Pakete yeni bir ürün bileşeni ekler
        public void AddComponent(Product product) => Components.Add(product);

        // Paketin toplam fiyatını, içindeki tüm ürünlerin fiyatlarını toplayarak hesaplar.
        public override decimal GetPrice() => Components.Sum(p => p.GetPrice());

        // Paketin stok durumunu hesaplar. 
        // Bir paketin stoku, içindeki en az stoka sahip olan bileşenin miktarı kadardır.
        public override int GetStock()
        {
            if (!Components.Any()) return 0;
            return Components.Min(p => p.GetStock());
        }

        // Paketin içeriğini listeleyen detaylı bir açıklama döndürür.
        public override string GetDescription()
        {
            if (!Components.Any()) return Name;

            return $"{Name} includes: " + string.Join(", ", Components.Select(c => c.Name));
        }

        /// Paket satıldığında, içindeki her bir bileşenin stok miktarını azaltır.
        /// Stok eşik değerinin altına düşerse gözlemcileri (Observers) uyarır.
        public override void DecreaseStock(int quantity)
        {
            foreach (var component in Components)
            {
                component.DecreaseStock(quantity);
            }

            // Paket genelindeki stok durumu kritik seviyeye inerse bildirim gönderir
            if (GetStock() <= Threshold)
            {
                NotifyObservers();
            }
        }

        // Paketi oluşturan her bir bileşenin stok miktarını artırır.
        public override void IncreaseStock(int quantity)
        {
            foreach (var component in Components)
            {
                component.IncreaseStock(quantity);
            }
        }

        // Paketin satışa uygun olup olmadığını kontrol eder.
        // Tüm bileşenlerin stokta mevcut olması gerekir.
        public bool IsAvailable()
        {
            return Components.Any() && Components.All(p => p.GetStock() > 0);
        }
    }
}