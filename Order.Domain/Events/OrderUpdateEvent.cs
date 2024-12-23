using BuildBlocks.Domain.Abstractions;

namespace Order.Domain.Events
{
    public class OrderUpdateEvent : BaseEvent
    {
        public Guid OrderId { get; private set; }
        public string OrderStatus { get; private set; }
        public OrderUpdateEvent(Guid orderId, string orderStatus)
        {
            OrderId = orderId;
            OrderStatus = orderStatus;
        }
    }
}
