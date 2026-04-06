
using BuildBlocks.Domain.Abstractions;
using BuildBlocks.Domain.Exceptions;
using Order.Domain.Enums;
using Order.Domain.Exceptions;
using Order.Domain.Extensions;

namespace Order.Domain.ValueObjects
{
    public class Delivery : ValueObject
    {
        required public DeliveryType Type { get; init; }
        required public DateOnly EstimatedDeliveryDate { get; init; }
        required public decimal Price { get; init; }

        public static Delivery Create(DeliveryType type, DateOnly estimatedDeliveryDate, decimal price)
        {
            var erros = new List<string>();
            
            erros.AddIf(estimatedDeliveryDate < DateOnly.FromDateTime(DateTime.Today), "A data de entrega estimada não pode ser retroativa")
                .AddIf(price < 0, "O preço de entrega não pode ser negativo");

            DomainException.ThrowIfAnyErro(erros, "Dados de entrega inválidos");

            return new Delivery()
            {
                Type = type,
                EstimatedDeliveryDate = estimatedDeliveryDate,
                Price = price
            };
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Type;
            yield return EstimatedDeliveryDate;
            yield return Price;
        }
    }
}
