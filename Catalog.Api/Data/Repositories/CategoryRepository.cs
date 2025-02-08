
using Catalog.Api.Data.Interfaces;
using Catalog.Api.Entities;

namespace Catalog.Api.Data.Repositories
{
    public sealed class CategoryRepository(CatalogContext catalogContext)
        : ReadRepository<CategoryEntity, Guid>(catalogContext), ICategoryRepository
    {
    }
}
