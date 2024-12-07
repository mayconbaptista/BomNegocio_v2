using Mapster;

namespace Catalog.WebApi.Dtos
{
    public static class MapsterConfig
    {
        public static void Configure()
        {
            #region "Product"
            TypeAdapterConfig<ProductModel, ProductListDto>
                .NewConfig()
                .Map(dest => dest.Name, src => src.Name)
                .Map(dest => dest.Price, src => src.Price)
                .Map(dest => dest.CategoryId, src => src.CategoryId);

            TypeAdapterConfig<ProductListDto, ProductModel>
                .NewConfig()
                .Map(dest => dest.Name, src => src.Name)
                .Map(dest => dest.Price, src => src.Price)
                .Map(dest => dest.CategoryId, src => src.CategoryId);

            TypeAdapterConfig<ProductModel, ProductDetailsDto>
                .NewConfig()
                .Map(dest => dest.Name, src => src.Name)
                .Map(dest => dest.Description, src => src.Description)
                .Map(dest => dest.Price, src => src.Price)
                .Map(dest => dest.CategoryId, src => src.CategoryId);
            #endregion
        }
    }
}
