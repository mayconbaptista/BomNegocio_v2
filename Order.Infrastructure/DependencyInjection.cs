
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Order.Domain.Interfaces;

namespace Order.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            var connection = configuration.GetConnectionString("DbConnection")
                ?? throw new ArgumentNullException("DbConnection is not found in the configuration file.");

            services.AddScoped<ISaveChangesInterceptor, DispatchDomainEventsInterceptor>();

            services.AddDbContext<OrderContext>((sp, options) =>
            {
                options.AddInterceptors(sp.GetRequiredService<ISaveChangesInterceptor>());
                options.UseNpgsql(connection, opt =>
                {
                    opt.SetPostgresVersion(new Version(16, 4));
                    opt.EnableRetryOnFailure(3);
                    opt.MigrationsAssembly(typeof(OrderContext).Assembly.FullName!);
                });
                options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
                options.EnableSensitiveDataLogging();
                options.EnableDetailedErrors();
                //options.UseLoggerFactory(LoggerFactory.Create(builder => builder.));
            });

            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }
    }
}
