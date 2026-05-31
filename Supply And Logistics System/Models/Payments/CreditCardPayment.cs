namespace Supply_And_Logistics_System.Models.Payments
{
    /// <summary>
    /// Kredi kartı ile ödeme yöntemini temsil eden sınıf.
    /// Strategy Pattern.
    /// </summary>
    public class CreditCardPayment : IPaymentStrategy
    {
        private readonly string _cardNumber;
        private readonly string _cardHolder;
        private readonly string _expiryDate;
        private readonly string _cvv;

        // Kredi kartı bilgilerini alarak ödeme stratejisini başlatır
        // Ödeme başarılıysa true, başarısızsa false döner<
        public CreditCardPayment(string cardNumber, string cardHolder, string expiryDate, string cvv)
        {
            _cardNumber = cardNumber;
            _cardHolder = cardHolder;
            _expiryDate = expiryDate;
            _cvv = cvv;
        }

        // Kredi kartı ödeme işlemini simüle eder.
        public bool ProcessPayment(decimal amount)
        {
            // Basit bir hata simülasyonu: Kart numarası "0000" ile bitiyorsa reddet.
            if (_cardNumber.EndsWith("0000"))
            {
                // Singleton Logger kullanılarak hata kaydı oluşturulur.
                Infrastructure.Logger.GetInstance()
                    .Log($"[CARD FAILED] Card declined. Amount: {amount}");
                return false;
            }

            Infrastructure.Logger.GetInstance()
                .Log($"[CARD APPROVED] Payment successful. Amount: {amount}");

            return true;
        }

        // Güvenlik nedeniyle kartın sadece son 4 hanesini ve sahibi bilgisini döndürür.
        public string GetPaymentDetails()
        {
            var last4 = _cardNumber.Length >= 4
                ? _cardNumber[^4..]
                : "****";

            return $"Credit Card **** **** **** {last4} - {_cardHolder}";
        }
    }
}