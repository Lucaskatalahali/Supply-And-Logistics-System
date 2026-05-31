using Supply_And_Logistics_System.Models.Products;
using Xunit;

namespace Supply_And_Logistics_System.Tests
{
    public class SimpleProductTests
    {
        [Fact]
        public void GetPrice_ReturnsCorrectPrice()
        {
            // Arrange
            var product = new SimpleProduct { Price = 25.50m };

            // Act
            var result = product.GetPrice();

            // Assert
            Assert.Equal(25.50m, result);
        }

        [Fact]
        public void GetStock_ReturnsCorrectStock()
        {
            // Arrange
            var product = new SimpleProduct { Stock = 10 };

            // Act
            var result = product.GetStock();

            // Assert
            Assert.Equal(10, result);
        }

        [Fact]
        public void DecreaseStock_ReducesStockCorrectly()
        {
            // Arrange
            var product = new SimpleProduct { Stock = 10, Threshold = 2 };

            // Act
            product.DecreaseStock(3);

            // Assert
            Assert.Equal(7, product.GetStock());
        }

        [Fact]
        public void IncreaseStock_IncreasesStockCorrectly()
        {
            // Arrange
            var product = new SimpleProduct { Stock = 10, Threshold = 2 };

            // Act
            product.IncreaseStock(5);

            // Assert
            Assert.Equal(15, product.GetStock());
        }
    }
}