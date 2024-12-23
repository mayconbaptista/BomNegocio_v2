using BuildBlocks.Domain.Abstractions;

namespace Cart.WebApi.Models
{
    public sealed class CartModel : BaseEntity<Guid>
    {
        public Guid CustomerId { get; set; }
        public List<CartItemModel> Items { get; set; } = new();
    }
}
