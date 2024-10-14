using Catalog.Domain.Enums;
using Catalog.Domain.Validations;
using FluentValidation;

namespace Catalog.Domain.Models
{
    public sealed class ProductModel : BaseModel
    {
        public string Name { get; set; }
        public string SkuCode { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public uint Amount { get; set; }
        public uint CategoryId { get; set; }
        public CategoryModel Category { get; set; }
        public ICollection<ImageModel> Images { get; set; } = new List<ImageModel>();

        public override void Validate()
        {
            var validator = new ProductValidator();
            var result = validator.Validate(this);

            if (!result.IsValid)
            {
                throw new ValidationException(result.Errors);
            }
        }
    }
}
