
namespace Catalog.Api.Entities
{
    public sealed class CategoryEntity : BaseEntity<Guid>
    {
        public string Name { get; private set; }
        public string Description { get; private set; }
        public Guid? ParentCategoryId { get; private set; }

        /* 1..N */
        public CategoryEntity ParentCategory { get; private set; }
        public ICollection<CategoryEntity> SubCategories { get; set; } = new List<CategoryEntity>();
        public ICollection<ProductEntity> Products { get; set; } = new List<ProductEntity>();
    }
}
