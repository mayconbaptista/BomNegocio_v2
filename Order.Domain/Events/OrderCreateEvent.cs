using BuildBlocks.Domain.Abstractions;
using Order.Domain.Entities;

namespace Order.Domain.Events
{
    public sealed class OrderCreateEvent(OrderEntity orderEntity) : BaseEvent
    {
        public OrderEntity OrderEntity { get; set; } = orderEntity;
    }
}
