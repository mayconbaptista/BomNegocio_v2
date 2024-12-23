
namespace BuildInBlocks.Messaging.Models
{
    public record CartCheckoutItem
    {
        public string ProductCode { get; init; }
        public string ProductName { get; init; }
        public decimal PriceUni { get; init; }
        public int Quantity { get; init; }
    }
}
