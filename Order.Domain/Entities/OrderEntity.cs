
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
        public decimal TotalPrice => OrderItems.Sum(i => i.UnitPrice * i.Quantity);


        public IReadOnlyList<OrderItemEntity> OrderItems => _OrderItems.AsReadOnly();
        private List<OrderItemEntity> _OrderItems { get; init; }

        public static OrderEntity Create(
            Guid customerId,
            Address shippingAddress,
            Address billingAddress)
        {
            var order = new OrderEntity
            {
                CustomerId = customerId,
                ShippingAddress = shippingAddress,
                BillingAddress = billingAddress
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

            this.AddDomainEvent(new OrderStatusChangedEvent(this.Id, this.Status.ToString()));
        }

        public void AddItem (Guid productId, uint quantity, decimal unitPrice)
        {
            if(Status != OrderStatus.Peding)
            {
                throw new InvalidOperationException("Não é possível adicionar itens a um pedido que não está pendente");
            }

            var item = OrderItemEntity.Create(productId,this.Id, quantity, unitPrice);

            _OrderItems.Add(item);
        }
    }
}
