using BuildBlocks.WebApi.Exceptions.Handlers;

namespace Order.WebApi
{
    public static class DependencyInjection
    {

        public static IServiceCollection AddApiServices (this IServiceCollection services, IConfiguration configuration)
        {
            services.AddExceptionHandler<CustomExceptionHandler>();

            return services;
        }
    }
}
