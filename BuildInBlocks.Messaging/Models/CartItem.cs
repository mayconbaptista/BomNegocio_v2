
namespace BuildInBlocks.Messaging.Models
{
    public record CartItem
    {
        public string ProductId { get; init; }
        public decimal UnitPrice { get; init; }
        public int Quantity { get; init; }
    }
}
