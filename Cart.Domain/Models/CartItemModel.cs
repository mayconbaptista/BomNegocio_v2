namespace Cart.Domain.Models
{
    public sealed class CartItemModel : BaseModel<Guid>
    {
        public Guid CartId { get; set; }
        public string ProductSkuCode { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}
