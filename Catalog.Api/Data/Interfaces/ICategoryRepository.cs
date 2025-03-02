using Catalog.Api.Entities;

namespace Catalog.Api.Data.Interfaces
{
    public interface ICategoryRepository : IReadRepository<CategoryEntity, Guid>
    {
    }
}
