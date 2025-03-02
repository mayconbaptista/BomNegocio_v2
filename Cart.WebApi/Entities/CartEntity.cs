using BuildBlocks.Domain.Abstractions;

namespace Cart.WebApi.Entities
{
    public sealed class CartEntity : BaseEntity<Guid>
    {
        public List<CartItemEntity> Items { get; set; } = new();
    }
}
