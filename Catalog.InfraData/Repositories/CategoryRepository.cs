
namespace Catalog.InfraData.Repositories
{
    public sealed class CategoryRepository(CatalogContext catalogContext)
        : ReadRepository<CategoryModel, uint>(catalogContext), ICategoryRepository
    {
    }
}
