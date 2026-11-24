
using BuildBlocks.Domain.Abstractions;
using BuildBlocks.Domain.Exceptions;
using BuildBlocks.Domain.ValueObjects;
using Order.Domain.Enums;
using Order.Domain.Events;
using Order.Domain.Exceptions;
using Order.Domain.Extensions;

namespace Order.Domain.Entities
{
    public sealed class OrderEntity : BaseAuditableEntity
    {
        required public string CostumerIdentifier { get;  init; }
        required public Address ShippingAddress { get; init; }
        required public Address BillingAddress { get; init; }
        required public Delivery Delivery { get; init; }
        public OrderStatus Status { get; private set; }
        required public PaymentEntity Payment { get; init; }
        public decimal TotalPrice => OrderItems.Sum(i => i.UnitPrice * i.Quantity);


        public IReadOnlyList<OrderItemEntity> OrderItems => _OrderItems.AsReadOnly();

        private readonly List<OrderItemEntity> _OrderItems = new();

        public static OrderEntity Create(
            string custumerIdentifier,
            Address shippingAddress,
            Address billingAddress,
            PaymentEntity payment,
            Delivery delivery)
        {

            if(custumerIdentifier.Length != 11 && custumerIdentifier.Length != 14)
            {
                throw new DomainException("O identificador do cliente deve conter 11 ou 14 caracteres");
            }

            var order = new OrderEntity
            {
                Status = OrderStatus.Pending,
                CostumerIdentifier = custumerIdentifier,
                ShippingAddress = shippingAddress,
                BillingAddress = billingAddress,
                Payment = payment,
                Delivery = delivery
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

            this.AddDomainEvent(new OrderStatusChangedEvent(this));
        }

        public void AddItem (Guid productId, uint quantity, decimal unitPrice)
        {
            if(Status != OrderStatus.Pending)
            {
                throw new InvalidOperationException("Não é possível adicionar itens a um pedido que não está pendente");
            }

            var item = OrderItemEntity.Create(productId,this.Id, quantity, unitPrice);

            _OrderItems.Add(item);
        }
    }
}
