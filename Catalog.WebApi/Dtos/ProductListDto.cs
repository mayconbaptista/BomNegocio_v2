namespace Catalog.WebApi.Dtos
{
    public record ProductListDto
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public uint CategoryId { get; set; }
    }
}
