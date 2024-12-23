
namespace Catalog.InfraData.Repositories
{
    public sealed class ImageRepository(CatalogContext catalogContext) 
        : WriteRepository<ImageModel, uint>(catalogContext), IImageRepository
    {

    }
}
