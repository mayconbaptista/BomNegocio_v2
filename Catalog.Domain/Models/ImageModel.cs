using BuildBlocks.Domain.Abstractions;

namespace Catalog.Domain.Models
{
    public sealed class ImageModel : BaseEntity<uint>
    {
        public string Path { get; set; }
        public uint ProductId { get; set; }
        public ProductModel Product { get; set; }

        public void Validate()
        {
            throw new NotImplementedException();
        }
    }
}
