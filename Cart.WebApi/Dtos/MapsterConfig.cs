using BuildBlocks.Domain.ValueObjects;
using Cart.WebApi.Dtos;
using System.Reflection;

namespace Order.Application.Dtos
{
    public static class MapsterConfig
    {
        public static IServiceCollection AddMapsterConfig (this IServiceCollection services)
        {

            TypeAdapterConfig<CartItemEntity, CartItemDto>
                .NewConfig()
                .Map(dest => dest.ProductId, src => src.ProductId)
                .Map(dest => dest.Quantity, src => src.Quantity);

            TypeAdapterConfig.GlobalSettings.Scan(Assembly.GetExecutingAssembly());

            return services;
        }
    }
}
