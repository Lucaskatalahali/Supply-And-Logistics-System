using Supply_And_Logistics_System.Models.Shipping;
using Supply_And_Logistics_System.Models.Shipping.Adapters;
using Supply_And_Logistics_System.Models.Shipping.Decorators;
using Xunit;

namespace Supply_And_Logistics_System.Tests
{
    public class ShippingTests
    {
        // =========================
        // ADAPTERS
        // =========================
        [Fact]
        public void ArasAdapter_CalculateCost_ReturnsPositiveValue()
        {
            var carrier = new ArasAdapter();
            var cost = carrier.CalculateCost(5, 100);
            Assert.True(cost > 0);
        }

        [Fact]
        public void YurticiAdapter_CalculateCost_ReturnsPositiveValue()
        {
            var carrier = new YurticiAdapter();
            var cost = carrier.CalculateCost(5, 100);
            Assert.True(cost > 0);
        }

        [Fact]
        public void GlobalExpressAdapter_CalculateCost_ReturnsPositiveValue()
        {
            var carrier = new GlobalExpressAdapter();
            var cost = carrier.CalculateCost(5, 100);
            Assert.True(cost > 0);
        }

        [Fact]
        public void ArasAdapter_GetTrackingNumber_StartsWithARAS()
        {
            var carrier = new ArasAdapter();
            var tracking = carrier.GetTrackingNumber();
            Assert.StartsWith("ARAS-", tracking);
        }

        [Fact]
        public void YurticiAdapter_GetTrackingNumber_StartsWithYURTICI()
        {
            var carrier = new YurticiAdapter();
            var tracking = carrier.GetTrackingNumber();
            Assert.StartsWith("YURTICI-", tracking);
        }

        [Fact]
        public void GlobalExpressAdapter_GetTrackingNumber_StartsWithGLOBAL()
        {
            var carrier = new GlobalExpressAdapter();
            var tracking = carrier.GetTrackingNumber();
            Assert.StartsWith("GLOBAL-", tracking);
        }

        // =========================
        // DECORATOR — INSURANCE
        // =========================
        [Fact]
        public void InsuranceDecorator_AddsFiftyEuros()
        {
            // Arrange
            var baseCarrier = new ArasAdapter();
            var baseCost = baseCarrier.CalculateCost(5, 100);
            var insured = new InsuranceDecorator(baseCarrier);

            // Act
            var insuredCost = insured.CalculateCost(5, 100);

            // Assert
            Assert.Equal(baseCost + 50m, insuredCost);
        }

        [Fact]
        public void InsuranceDecorator_KeepsOriginalTrackingNumber()
        {
            var baseCarrier = new ArasAdapter();
            var insured = new InsuranceDecorator(baseCarrier);

            Assert.StartsWith("ARAS-", insured.GetTrackingNumber());
        }

        // =========================
        // DECORATOR — FRAGILE
        // =========================
        [Fact]
        public void FragileDecorator_AddsTwentyFiveEuros()
        {
            var baseCarrier = new YurticiAdapter();
            var baseCost = baseCarrier.CalculateCost(5, 100);
            var fragile = new FragileProtectionDecorator(baseCarrier);

            var fragileCost = fragile.CalculateCost(5, 100);

            Assert.Equal(baseCost + 25m, fragileCost);
        }

        // =========================
        // AMBOS DECORATORS
        // =========================
        [Fact]
        public void BothDecorators_AddSeventyFiveEuros()
        {
            var baseCarrier = new ArasAdapter();
            var baseCost = baseCarrier.CalculateCost(5, 100);

            // aplica os dois decorators
            ICarrier decorated = new InsuranceDecorator(baseCarrier);
            decorated = new FragileProtectionDecorator(decorated);

            var finalCost = decorated.CalculateCost(5, 100);

            Assert.Equal(baseCost + 75m, finalCost); // 50 + 25
        }

        // =========================
        // FACTORY
        // =========================
        [Fact]
        public void CarrierFactory_GetCarrier_ReturnsArasAdapter()
        {
            var factory = new CarrierFactory();
            var carrier = factory.GetCarrier("aras");
            Assert.IsType<ArasAdapter>(carrier);
        }

        [Fact]
        public void CarrierFactory_GetCarrier_ReturnsYurticiAdapter()
        {
            var factory = new CarrierFactory();
            var carrier = factory.GetCarrier("yurtici");
            Assert.IsType<YurticiAdapter>(carrier);
        }

        [Fact]
        public void CarrierFactory_GetCarrier_ReturnsGlobalExpressAdapter()
        {
            var factory = new CarrierFactory();
            var carrier = factory.GetCarrier("globalexpress");
            Assert.IsType<GlobalExpressAdapter>(carrier);
        }

        [Fact]
        public void CarrierFactory_GetCarrier_ThrowsException_WhenInvalidType()
        {
            var factory = new CarrierFactory();
            Assert.Throws<Exception>(() => factory.GetCarrier("invalid"));
        }
    }
}