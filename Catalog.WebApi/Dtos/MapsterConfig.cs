using Mapster;

namespace Catalog.WebApi.Dtos
{
    public static class MapsterConfig
    {
        public static void Configure()
        {
            TypeAdapterConfig<ProductModel, ProductDto>
                .NewConfig()
                .Map(dest => dest.Name, src => src.Name)
                .Map(dest => dest.Description, src => src.Description)
                .Map(dest => dest.SkuCode, src => src.SkuCode)
                .Map(dest => dest.Price, src => src.Price)
                .Map(dest => dest.Amount, src => src.Amount)
                .Map(dest => dest.CategoryId, src => src.CategoryId);

            TypeAdapterConfig<ProductDto, ProductModel>
                .NewConfig()
                .Map(dest => dest.Name, src => src.Name)
                .Map(dest => dest.Description, src => src.Description)
                .Map(dest => dest.SkuCode, src => src.SkuCode)
                .Map(dest => dest.Price, src => src.Price)
                .Map(dest => dest.Amount, src => src.Amount)
                .Map(dest => dest.CategoryId, src => src.CategoryId);
        }
    }
}
