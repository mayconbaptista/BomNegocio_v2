using BuildBlocks.Domain.Abstractions;

namespace Cart.WebApi.Models
{
    public sealed class CartItemModel : BaseEntity<Guid>
    {
        public Guid CartId { get; set; }
        public string ProductSkuCode { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}
