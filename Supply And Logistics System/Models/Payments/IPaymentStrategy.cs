namespace Supply_And_Logistics_System.Models.Payments
{
    /// <summary>
    /// Ödeme işlemleri için temel strateji arayüzü.
    /// Strategy Pattern yapısındaki "Strategy" birimidir.
    /// Bu arayüz sayesinde sistem, ödeme yöntemlerinden (Kredi Kartı, Havale vb.) bağımsız çalışır.
    /// </summary>
    public interface IPaymentStrategy
    {
        /// Verilen tutar için ödeme işlemini gerçekleştirir.
        /// Farklı ödeme yöntemleri bu metodu kendi kurallarına göre uygular.
        bool ProcessPayment(decimal amount);

        /// Kullanılan ödeme yöntemiyle ilgili açıklayıcı bilgileri döndürür.
        string GetPaymentDetails();
    }
}