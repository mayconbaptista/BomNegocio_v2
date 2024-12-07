namespace Cart.Domain.Models
{
    public sealed class CartItemModel : BaseModel<Guid>
    {
        public Guid CartId { get; set; }
        public string ProductSkoCode { get; set; }
        public int Quantity { get; set; }
    }
}
