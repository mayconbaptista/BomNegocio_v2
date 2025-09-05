using Catalog.Api.Entities;
using Catalog.Api.ProductEndPoints.CreateProduct;
using Mapster;

namespace Catalog.Api.Dtos
{
    public static class MapsterConfig
    {
        public static void RegisterMappings()
        {
            #region "Product"
            TypeAdapterConfig<ProductEntity, ProductDto>
                .NewConfig()
                .Map(dest => dest.Id, src => src.Id)
                .Map(dest => dest.Name, src => src.Name)
                .Map(dest => dest.Price, src => src.Price)
                .Map(dest => dest.Description, src => src.Description)
                .Map(dest => dest.Image, src => src.Images.Count > 0 ? src.Images.FirstOrDefault()!.path : string.Empty)
                .Map(dest => dest.Category, src => src.Category.Name);

            TypeAdapterConfig<ProductEntity, ProductDetailsDto>
                .NewConfig()
                .Map(dest => dest.Id, src => src.Id)
                .Map(dest => dest.Name, src => src.Name)
                .Map(dest => dest.Description, src => src.Description)
                .Map(dest => dest.Price, src => src.Price)
                .Map(dest => dest.image, src => src.Images.Count > 0 ? src.Images.FirstOrDefault()!.path : string.Empty)
                .Map(dest => dest.Category, src => src.Category.Name);

            #endregion
            #region "Request_src to ..."
            TypeAdapterConfig<CreateProductCommand, ProductEntity>
                .NewConfig()
                .Map(dest => dest.Name, src => src.Name)
                .Map(dest => dest.SkuCode, src => src.SkuCode)
                .Map(dest => dest.Description, src => src.Description)
                .Map(dest => dest.Price, src => src.Price)
                .Map(dest => dest.Quantity, src => src.Quantity)
                .Map(dest => dest.CategoryId, src => src.CategoryId);
            #endregion
        }
    }
}
