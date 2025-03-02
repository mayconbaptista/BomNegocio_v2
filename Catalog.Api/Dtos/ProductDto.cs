namespace Catalog.Api.Dtos
{
    public record ProductDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public uint Quantity { get; set; }
        public Guid CategoryId { get; set; }
    }
}
