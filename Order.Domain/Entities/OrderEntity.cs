
using BuildBlocks.Domain.Abstractions;
using BuildBlocks.Domain.ValueObjects;
using Order.Domain.Enums;
using Order.Domain.Events;

namespace Order.Domain.Entities
{
    public sealed class OrderEntity : BaseAuditableEntity
    {
        public Guid CustomerId { get; private set; }
        public Address ShippingAddress { get; private set; } = default!;
        public Address BillingAddress { get; private set; } = default!;
        public OrderStatus Status { get; private set; } = OrderStatus.Peding;
        public decimal TotalPrice
        {
            get => OrderItems.Sum(i => i.UnitPrice * i.Quantity);
            private set { }
        }

        public IReadOnlyList<OrderItemEntity> OrderItems => _OrderItems.AsReadOnly();
        private List<OrderItemEntity> _OrderItems { get; init; }

        public static OrderEntity Create(
            Guid customerId,
            Address shippingAddress,
            Address billingAddress,
            List<OrderItemEntity> items)
        {
            var order = new OrderEntity
            {
                CustomerId = customerId,
                ShippingAddress = shippingAddress,
                BillingAddress = billingAddress,
                _OrderItems = items,
                CreateAt = DateTime.UtcNow,
                LastModifiedAt = null
            };

            order.AddDomainEvent(new OrderCreateEvent(order));

            return order;
        }

        public void UpdateStatus(OrderStatus status)
        {
            if (status <= Status)
            {
                throw new InvalidOperationException("O Status da ordem não pode retoceder");
            }

            Status = status;
            LastModifiedAt = DateTime.UtcNow;

            this.AddDomainEvent(new OrderStatusChangedEvent(this.Id, this.Status.ToString()));
        }
    }
}
