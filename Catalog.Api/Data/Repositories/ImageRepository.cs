
using Catalog.Api.Data.Interfaces;
using Catalog.Api.Entities;

namespace Catalog.Api.Data.Repositories
{
    public sealed class ImageRepository(CatalogContext catalogContext) 
        : WriteRepository<ImageEntity, uint>(catalogContext), IImageRepository
    {

    }
}
