
using BuildBlocks.Domain.Abstractions;
using Order.Domain.Exceptions;
using Order.Domain.Extensions;

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
            List<string> errors = new List<string>();

            errors.AddIf(productId == Guid.Empty, "O identitificador do produdo é obrigatório.")
                .AddIf(orderId == Guid.Empty, "O identitificador do pedido é obrigatório.")
                .AddIf(quantity == 0, "A quantidade deve ser maior que zero.")
                .AddIf(unitPrice <= 0, "O preço unitário deve ser maior que zero.");

            DomainException.ThrowIfAnyErro(errors, "Item do pedido inválido");

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
