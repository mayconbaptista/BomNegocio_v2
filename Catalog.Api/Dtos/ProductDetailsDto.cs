namespace Catalog.Api.Dtos
{
    public record ProductDetailsDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public string image { get; set; }
        public RatingDto Rating { get; set; } = new RatingDto();
    }
}
