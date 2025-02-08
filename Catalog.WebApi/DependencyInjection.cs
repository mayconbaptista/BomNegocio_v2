using BuildBlocks.WebApi.Exceptions.Handlers;

namespace Catalog.Api
{
    public static class DependencyInjection
    {

        public static IServiceCollection AddApiServices (this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();


            return services;
        }
    }
}
