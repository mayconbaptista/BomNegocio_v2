
using BuildBlocks.Domain.Abstractions;
using Order.Domain.Enums;

namespace Order.Domain.ValueObjects
{
    public class Delivery : ValueObject
    {
        public DeliveryType Type { get; set; }
        public DateOnly EstimatedDeliveryDate { get; private set; }
        public decimal Price { get; private set; }

        public static Delivery Create(DeliveryType type, DateOnly estimatedDeliveryDate, decimal price)
        {
            if(estimatedDeliveryDate <= DateOnly.FromDateTime(DateTime.UtcNow))
            {
                throw new ArgumentException("A data de entrega estimada não pode ser uma data passada.");
            }

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
