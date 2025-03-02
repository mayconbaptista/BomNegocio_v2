using BuildBlocks.Domain.Abstractions;

namespace Cart.WebApi.Entities
{
    public sealed class CartItemEntity : BaseEntity<Guid>
    {
        public Guid ProductId { get; set; }
        public Guid CartId { get; set; }
        public uint Quantity { get; set; }
    }
}
