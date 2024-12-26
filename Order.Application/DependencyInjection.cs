
using Microsoft.Extensions.DependencyInjection;
using BuildInBlocks.Messaging.Extensions;
using Microsoft.Extensions.Configuration;
using System.Reflection;
using Microsoft.FeatureManagement;

namespace Order.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMediatR(config =>
            {
                config.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly);
            });

            services.AddFeatureManagement(configuration);
            services.AddMassTransit(configuration, Assembly.GetExecutingAssembly());

            return services;
        }
    }
}
