using BuildBlocks.Domain.Abstractions;

namespace Order.Domain.Events
{
    public class OrderStatusChangedEvent : BaseEvent
    {
        public OrderEntity Order { get; private set; }

        public OrderStatusChangedEvent(OrderEntity order)
        {
            Order = order;
        }
    }
}
