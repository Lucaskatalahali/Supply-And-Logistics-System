using Supply_And_Logistics_System.Infrastructure;
using Xunit;

namespace Supply_And_Logistics_System.Tests
{
    public class LoggerTests
    {
        [Fact]
        public void Logger_GetInstance_ReturnsSameInstance()
        {
            // Arrange & Act
            var instance1 = Logger.GetInstance();
            var instance2 = Logger.GetInstance();

            // Assert — Singleton: as duas variáveis apontam para o mesmo objeto
            Assert.Same(instance1, instance2);
        }

        [Fact]
        public void Logger_GetInstance_IsNotNull()
        {
            var instance = Logger.GetInstance();
            Assert.NotNull(instance);
        }
    }
}