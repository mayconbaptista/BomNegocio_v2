namespace Catalog.WebApi.Dtos
{
    public record ProductDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string SkuCode { get; set; }
        public decimal Price { get; set; }
        public uint Amount { get; set; }
        public uint CategoryId { get; set; }
    }
}
