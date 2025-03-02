using BuildBlocks.Domain.Abstractions;

namespace Catalog.Api.Entities
{
    public sealed class ProductEntity : BaseEntity<Guid>
    {
        public string Name { get; set; }
        public string SkuCode { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public uint Quantity { get; set; }
        public Guid CategoryId { get; set; }
        public CategoryEntity Category { get; set; }
        public ICollection<ImageEntity> Images { get; set; } = new List<ImageEntity>();
    }
}
