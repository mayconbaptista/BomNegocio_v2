
using Microsoft.Extensions.DependencyInjection;
using BuildInBlocks.Messaging.Extensions;
using Microsoft.Extensions.Configuration;
using System.Reflection;

namespace Order.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMediatR( config =>
            {
                config.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly);
            });

            services.AddMassTransit(configuration, Assembly.GetExecutingAssembly());

            return services;
        }
    }
}
