namespace Cart.WebApi.Dtos
{
    public record CartItemDto
    {
        public Guid ProductId { get; init; }
        public uint Quantity { get; init; }
    }
}
