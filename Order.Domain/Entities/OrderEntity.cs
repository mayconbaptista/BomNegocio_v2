
using BuildBlocks.Domain.Abstractions;
using BuildBlocks.Domain.ValueObjects;
using Order.Domain.Enums;

namespace Order.Domain.Models
{
    public sealed class OrderEntity : BaseAuditableEntity<Guid>
    {
        public Address ShippingAddress { get; init; }
        public Customer Customer { get; init; }
        public IReadOnlyList<OrderItem> Items { get; init; }
        public OrderStatus Status { get; private set; }
        public decimal Value { get; init; }

        public OrderEntity(
            Address shippingAddress, 
            Customer customer, 
            List<OrderItem> items, 
            decimal value)
        {
            Id = Guid.NewGuid();
            ShippingAddress = shippingAddress;
            Customer = customer;
            Items = items;
            Value = value;
            CreateAt = DateTime.UtcNow;
            LastModifiedAt = null;
        }

        public void UpdateStatus(OrderStatus status) 
        {
            if(status <= this.Status)
            {
                throw new InvalidOperationException("O Status da ordem não pode retoceder");
            }

            this.Status = status;
            this.LastModifiedAt = DateTime.UtcNow;
        }
    }
}
