using Supply_And_Logistics_System.Models.Orders;
using Supply_And_Logistics_System.Models.Orders.States;
using Xunit;

namespace Supply_And_Logistics_System.Tests
{
    public class OrderStateTests
    {
        private Order CreateOrder(IOrderState state)
        {
            var order = new Order();
            order.SetState(state);
            return order;
        }

        // =========================
        // PENDING STATE
        // =========================
        [Fact]
        public void PendingState_NextState_MovesToConfirmed()
        {
            var order = CreateOrder(new PendingState());
            order.LoadState();
            order.NextStep();
            Assert.Equal("ConfirmedState", order.StateName);
        }

        [Fact]
        public void PendingState_Cancel_MovesToCancelled()
        {
            var order = CreateOrder(new PendingState());
            order.LoadState();
            order.Cancel();
            Assert.Equal("CancelledState", order.StateName);
        }

        [Fact]
        public void PendingState_RequestReturn_ThrowsException()
        {
            var order = CreateOrder(new PendingState());
            order.LoadState();
            Assert.Throws<InvalidOperationException>(() => order.RequestReturn());
        }

        // =========================
        // CONFIRMED STATE
        // =========================
        [Fact]
        public void ConfirmedState_NextState_MovesToInPreparation()
        {
            var order = CreateOrder(new ConfirmedState());
            order.LoadState();
            order.NextStep();
            Assert.Equal("InPreparationState", order.StateName);
        }

        [Fact]
        public void ConfirmedState_Cancel_MovesToCancelled()
        {
            var order = CreateOrder(new ConfirmedState());
            order.LoadState();
            order.Cancel();
            Assert.Equal("CancelledState", order.StateName);
        }

        // =========================
        // IN PREPARATION STATE
        // =========================
        [Fact]
        public void InPreparationState_NextState_MovesToInTransit()
        {
            var order = CreateOrder(new InPreparationState());
            order.LoadState();
            order.NextStep();
            Assert.Equal("InTransitState", order.StateName);
        }

        [Fact]
        public void InPreparationState_Cancel_MovesToCancelled()
        {
            var order = CreateOrder(new InPreparationState());
            order.LoadState();
            order.Cancel();
            Assert.Equal("CancelledState", order.StateName);
        }

        // =========================
        // IN TRANSIT STATE
        // =========================
        [Fact]
        public void InTransitState_NextState_MovesToDelivered()
        {
            var order = CreateOrder(new InTransitState());
            order.LoadState();
            order.NextStep();
            Assert.Equal("DeliveredState", order.StateName);
        }

        [Fact]
        public void InTransitState_Cancel_ThrowsException()
        {
            var order = CreateOrder(new InTransitState());
            order.LoadState();
            Assert.Throws<InvalidOperationException>(() => order.Cancel());
        }

        [Fact]
        public void InTransitState_RequestReturn_MovesToReturnState()
        {
            var order = CreateOrder(new InTransitState());
            order.LoadState();
            order.RequestReturn();
            Assert.Equal("ReturnState", order.StateName);
        }

        // =========================
        // DELIVERED STATE
        // =========================
        [Fact]
        public void DeliveredState_Cancel_ThrowsException()
        {
            var order = CreateOrder(new DeliveredState());
            order.LoadState();
            Assert.Throws<InvalidOperationException>(() => order.Cancel());
        }

        [Fact]
        public void DeliveredState_RequestReturn_ThrowsException()
        {
            var order = CreateOrder(new DeliveredState());
            order.LoadState();
            Assert.Throws<InvalidOperationException>(() => order.RequestReturn());
        }

        // =========================
        // RETURN STATE
        // =========================
        [Fact]
        public void ReturnState_NextState_MovesToInPreparation()
        {
            var order = CreateOrder(new ReturnState());
            order.LoadState();
            order.NextStep();
            Assert.Equal("InPreparationState", order.StateName);
        }
    }
}