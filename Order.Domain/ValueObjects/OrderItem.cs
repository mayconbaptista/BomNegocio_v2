
using BuildBlocks.Domain.Abstractions;

namespace Order.Domain.ValueObjects
{
    public class OrderItem : ValueObject
    {
        public int Quantity { get; private set; }
        public decimal Price { get; private set; }
        public string ProductName { get; private set; }
        public string ProductSku { get; private set; }

        public OrderItem(int quantity, decimal price, string productName, string productSku)
        {
            Quantity = quantity;
            Price = price;
            ProductName = productName;
            ProductSku = productSku;
        }
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Quantity;
            yield return Price;
            yield return ProductName;
        }
    }
}
