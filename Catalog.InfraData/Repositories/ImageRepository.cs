
namespace Catalog.InfraData.Repositories
{
    public sealed class ImageRepository(CatalogContext catalogContext) 
        : WriteRepository<ImageModel>(catalogContext), IImageRepository
    {

    }
}
