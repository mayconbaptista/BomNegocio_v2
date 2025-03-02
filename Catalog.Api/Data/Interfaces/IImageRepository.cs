using Catalog.Api.Entities;

namespace Catalog.Api.Data.Interfaces
{
    public interface IImageRepository : IWriteRepository<ImageEntity, uint>
    {

    }
}
