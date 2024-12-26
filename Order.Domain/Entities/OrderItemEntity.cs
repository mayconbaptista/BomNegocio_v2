
using BuildBlocks.Domain.Abstractions;

namespace Order.Domain.Entities
{
    public class OrderItemEntity : BaseEntity<Guid>
    {
        public Guid ProductId { get; private set; }
        public Guid OrderId { get; private set; }
        public int Quantity { get; private set; }
        public decimal UnitPrice { get; private set; }

        public static OrderItemEntity Create (Guid productId, int quantity, decimal unitPrice)
        {
            return new OrderItemEntity
            {
                ProductId = productId,
                Quantity = quantity,
                UnitPrice = unitPrice
            };
        }
    }
}
