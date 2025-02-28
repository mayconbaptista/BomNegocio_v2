using BuildBlocks.Domain.Abstractions;

namespace Order.Domain.Events
{
    public sealed class OrderCreateEvent : BaseEvent
    {
        public OrderEntity Order { get; private init; }
        public OrderCreateEvent(OrderEntity order)
        {
            Order = order;
        }
    }
}
