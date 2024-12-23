
using BuildBlocks.Domain.Abstractions;

namespace Catalog.Domain.Models
{
    public sealed class CategoryModel : BaseEntity<uint>
    {
        public string Name { get; private set; }
        public string Description { get; private set; }
        public uint? ParentCategoryId { get; private set; }

        /* 1..N */
        public CategoryModel? ParentCategory { get; private set; }
        public ICollection<CategoryModel> SubCategories { get; set; } = new List<CategoryModel>();
        public ICollection<ProductModel> Products { get; set; } = new List<ProductModel>();

        public void Validate()
        {
            throw new NotImplementedException();
        }
    }
}
