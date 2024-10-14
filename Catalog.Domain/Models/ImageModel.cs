namespace Catalog.Domain.Models
{
    public sealed class ImageModel : BaseModel
    {
        public string Path { get; set; }
        public uint ProductId { get; set; }
        public ProductModel Product { get; set; }

        public override void Validate()
        {
            throw new NotImplementedException();
        }
    }
}
