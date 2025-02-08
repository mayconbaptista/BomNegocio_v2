using BuildBlocks.Domain.Abstractions;

namespace Catalog.Api.Entities
{
    public sealed class ImageEntity : BaseEntity<uint>
    {
        public string Path { get; set; }
        public Guid ProductId { get; set; }
        public ProductEntity Product { get; set; }
    }
}
