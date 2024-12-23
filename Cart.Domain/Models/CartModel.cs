namespace Cart.Domain.Models
{
    public sealed class CartModel : BaseModel<Guid>
    {
        public Guid CustomerId { get; set; }
        public List<CartItemModel> Items { get; set; } = new();
    }
}
