namespace Supply_And_Logistics_System.Models.Payments
{
    /// <summary>
    /// Banka havalesi ile ödeme yöntemini temsil eden sınıf.
    /// Strategy Pattern.
    /// </summary>
    public class BankTransferPayment : IPaymentStrategy
    {
        // Ödeme sürecini banka havalesi kurallarına göre işler.
        // Bu yöntemde ödeme anlık değil, manuel onay bekleyen bir niyet olarak kaydedilir.
        public bool ProcessPayment(decimal amount)
        {
            // Ödeme anında doğrulanmaz, sadece sistem kaydı oluşturulur.
            // Bu aşamadan sonra sipariş Pending durumuna geçer.

            Infrastructure.Logger.GetInstance()
                .Log($"[BANK TRANSFER] Order marked for manual payment. Amount: {amount}");

            return true; // Always true
        }

        // Kullanıcıya ödeme yöntemiyle ilgili bilgi mesajı döndürür.
        public string GetPaymentDetails()
        {
            return "Bank Transfer selected. Awaiting admin approval.";
        }
    }
}