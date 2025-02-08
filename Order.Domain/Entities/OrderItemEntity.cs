
using BuildBlocks.Domain.Abstractions;

namespace Order.Domain.Entities
{
    public class OrderItemEntity : BaseEntity<Guid>
    {
        public Guid ProductId { get; private set; }
        public Guid OrderId { get; private set; }
        public uint Quantity { get; private set; }
        public decimal UnitPrice { get; private set; }

        public static OrderItemEntity Create (Guid productId,Guid orderId, uint quantity, decimal unitPrice)
        {
            return new OrderItemEntity
            {
                ProductId = productId,
                OrderId = orderId,
                Quantity = quantity,
                UnitPrice = unitPrice
            };
        }
    }
}
