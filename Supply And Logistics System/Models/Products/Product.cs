namespace Supply_And_Logistics_System.Models.Products
{
    /// <summary>
    /// Ürün hiyerarşisi için temel soyut sınıf (Abstract Class).
    /// Composite Pattern (Bileşik Desen) ve Observer Pattern (Gözlemci Deseni) temelini oluşturur.
    public abstract class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        // SimpleProduct için veritabanından gelen değerdir. 
        // CompositeProduct için bu değerler dinamik hesaplama lehine göz ardı edilir.
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public int Threshold { get; set; }
        public string Description { get; set; } = string.Empty;

        // Stok değişikliklerini takip eden gözlemcilerin listesi.
        private List<IStockObserver> _observers = new();

        // Métodos Abstratos (Obrigatórios para os filhos)
        public abstract decimal GetPrice();
        public abstract int GetStock();

        // Yeni bir gözlemci (Email, Bildirim vb.) ekler.
        public void AddObserver(IStockObserver observer) => _observers.Add(observer);

        // Stok durumu değiştiğinde tüm kayıtlı gözlemcilere haber verir.
        public void NotifyObservers()
        {
            foreach (var observer in _observers)
            {
                observer.UpdateStock(Name, GetStock());
            }
        }

        public virtual string GetDescription()
        {
            return Description;
        }

        // Stok miktarını azaltır. Eğer stok eşik değerinin altına düşerse gözlemcileri uyarır.
        public virtual void DecreaseStock(int quantity)
        {
            Stock -= quantity;
            // Stok eşik değerine ulaştığında veya altına düştüğünde bildirim gönderir
            if (GetStock() <= Threshold)
            {
                NotifyObservers();
            }
        }

        // Stok miktarını artırır.
        public virtual void IncreaseStock(int quantity) => Stock += quantity;
    }
}
