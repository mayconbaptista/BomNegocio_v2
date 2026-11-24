namespace Catalog.Api.Dtos
{
    public record RatingDto
    {
        public decimal Rate { get; set; } = 5;
        public decimal Countt { get; set; } = 2;
    }
}
