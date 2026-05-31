using Supply_And_Logistics_System.Models.Payments;
using Xunit;

namespace Supply_And_Logistics_System.Tests
{
    public class PaymentTests
    {
        // =========================
        // CREDIT CARD
        // =========================
        [Fact]
        public void CreditCard_ProcessPayment_ReturnsTrue_WhenValidCard()
        {
            // Arrange
            var payment = new CreditCardPayment("4111111111111234", "John Doe", "12/26", "123");

            // Act
            var result = payment.ProcessPayment(100m);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void CreditCard_ProcessPayment_ReturnsFalse_WhenCardEndsWith0000()
        {
            // Arrange
            var payment = new CreditCardPayment("4111111111110000", "John Doe", "12/26", "123");

            // Act
            var result = payment.ProcessPayment(100m);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void CreditCard_GetPaymentDetails_ContainsLastFourDigits()
        {
            // Arrange
            var payment = new CreditCardPayment("4111111111111234", "John Doe", "12/26", "123");

            // Act
            var details = payment.GetPaymentDetails();

            // Assert
            Assert.Contains("1234", details);
        }

        // =========================
        // BANK TRANSFER
        // =========================
        [Fact]
        public void BankTransfer_ProcessPayment_AlwaysReturnsTrue()
        {
            // Arrange
            var payment = new BankTransferPayment();

            // Act
            var result = payment.ProcessPayment(500m);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void BankTransfer_GetPaymentDetails_ReturnsNonEmptyString()
        {
            // Arrange
            var payment = new BankTransferPayment();

            // Act
            var details = payment.GetPaymentDetails();

            // Assert
            Assert.False(string.IsNullOrEmpty(details));
        }
    }
}