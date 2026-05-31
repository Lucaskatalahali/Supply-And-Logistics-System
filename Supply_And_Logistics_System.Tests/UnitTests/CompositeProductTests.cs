using Supply_And_Logistics_System.Models.Products;
using Xunit;

namespace Supply_And_Logistics_System.Tests
{
    public class CompositeProductTests
    {
        [Fact]
        public void GetPrice_ReturnsSumOfComponentPrices()
        {
            // Arrange
            var ram = new SimpleProduct { Name = "RAM", Price = 50m, Stock = 10, Threshold = 2 };
            var cpu = new SimpleProduct { Name = "CPU", Price = 100m, Stock = 10, Threshold = 2 };
            var pc = new CompositeProduct { Name = "PC" };
            pc.AddComponent(ram);
            pc.AddComponent(cpu);

            // Act
            var result = pc.GetPrice();

            // Assert
            Assert.Equal(150m, result);
        }

        [Fact]
        public void GetStock_ReturnsMinimumStockAmongComponents()
        {
            // Arrange
            var ram = new SimpleProduct { Name = "RAM", Price = 50m, Stock = 5, Threshold = 2 };
            var cpu = new SimpleProduct { Name = "CPU", Price = 100m, Stock = 3, Threshold = 2 };
            var pc = new CompositeProduct { Name = "PC" };
            pc.AddComponent(ram);
            pc.AddComponent(cpu);

            // Act
            var result = pc.GetStock();

            // Assert
            Assert.Equal(3, result); // mínimo entre 5 e 3
        }

        [Fact]
        public void GetStock_ReturnsZero_WhenNoComponents()
        {
            // Arrange
            var pc = new CompositeProduct { Name = "PC" };

            // Act
            var result = pc.GetStock();

            // Assert
            Assert.Equal(0, result);
        }

        [Fact]
        public void DecreaseStock_DecreasesAllComponents()
        {
            // Arrange
            var ram = new SimpleProduct { Name = "RAM", Price = 50m, Stock = 10, Threshold = 2 };
            var cpu = new SimpleProduct { Name = "CPU", Price = 100m, Stock = 10, Threshold = 2 };
            var pc = new CompositeProduct { Name = "PC", Threshold = 1 };
            pc.AddComponent(ram);
            pc.AddComponent(cpu);

            // Act
            pc.DecreaseStock(2);

            // Assert
            Assert.Equal(8, ram.GetStock());
            Assert.Equal(8, cpu.GetStock());
        }

        [Fact]
        public void AddComponent_IncreasesComponentCount()
        {
            // Arrange
            var pc = new CompositeProduct { Name = "PC" };
            var ram = new SimpleProduct { Name = "RAM", Price = 50m, Stock = 10, Threshold = 2 };

            // Act
            pc.AddComponent(ram);

            // Assert
            Assert.Single(pc.Components);
        }
    }
}