namespace Cart.Domain.Models
{
    public sealed class CartModel : BaseModel<Guid>
    {
        public string CustomerId { get; set; }
        public string? ShippingAdressId { get; set; }
        public string? BillingAdressId { get; set; }
    }
}
