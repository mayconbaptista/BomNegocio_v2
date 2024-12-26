using BuildBlocks.Domain.Abstractions;

namespace Order.Domain.Events
{
    public class OrderStatusChangedEvent : BaseEvent
    {
        public Guid OrderId { get; private set; }
        public string OrderStatus { get; private set; }
        public OrderStatusChangedEvent(Guid orderId, string orderStatus)
        {
            OrderId = orderId;
            OrderStatus = orderStatus;
        }
    }
}
