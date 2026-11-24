
namespace Catalog.Api.Entities
{
    public sealed class CategoryEntity : BaseEntity<Guid>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid? ParentCategoryId { get; set; }

        /* 1..N */
        public CategoryEntity ParentCategory { get; set; }
        public ICollection<CategoryEntity> SubCategories { get; set; } = new List<CategoryEntity>();
        public ICollection<ProductEntity> Products { get; set; } = new List<ProductEntity>();
    }
}
