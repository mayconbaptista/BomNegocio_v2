using BuildBlocks.Domain.Abstractions;

namespace Order.Domain.Events
{
    public class OrderCanceledEvent : BaseEvent
    {
        public Guid OrderId { get; private set; }
        public List<OrderItemEntity> OrderItens { get; private set; } 
        public OrderCanceledEvent(Guid orderId, List<OrderItemEntity> orderItens)
        {
            OrderId = orderId;
            OrderItens = orderItens;
        }
    }
}
