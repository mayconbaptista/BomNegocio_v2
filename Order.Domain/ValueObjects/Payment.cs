
using BuildBlocks.Domain.Abstractions;

namespace Order.Domain.ValueObjects
{
    public class Payment : ValueObject
    {
        public string Currency { get; private set; }
        public decimal Amount { get; private set; }

        private Payment(string currency, decimal amount)
        {
            Currency = currency;
            Amount = amount;
        }

        public static Payment Create(string currency, decimal amount)
        {
            return new Payment(currency, amount);
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Currency;
            yield return Amount;
        }
    }
}
