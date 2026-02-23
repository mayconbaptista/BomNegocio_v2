namespace Cart.WebApi.Dtos
{
    public record CartProductDto
    {
        public CartItemDto Product { get; set; }

        public uint Quantity { get; set; }
    }
}
