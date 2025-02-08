using Catalog.Api.Entities;
using Catalog.Api.Product.CreateProduct;
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
                .Map(dest => dest.CategoryId, src => src.CategoryId);

            TypeAdapterConfig<ProductEntity, ProductDetailsDto>
                .NewConfig()
                .Map(dest => dest.Id, src => src.Id)
                .Map(dest => dest.Name, src => src.Name)
                .Map(dest => dest.Description, src => src.Description)
                .Map(dest => dest.Price, src => src.Price)
                .Map(dest => dest.CategoryId, src => src.CategoryId);
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
