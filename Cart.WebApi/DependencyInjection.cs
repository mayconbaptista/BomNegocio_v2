using Microsoft.EntityFrameworkCore;

namespace Cart.WebApi
{
    public static class DependencyInjection 
    {
        public static IServiceCollection RegisterServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddScoped<ExtractIdentifierFromTokenEndPointFilter>();

            #region "Services"
            services.AddScoped<ICartRepository, CartRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            #endregion

            var connection = config.GetConnectionString("DbConnection")
                ?? throw new ArgumentNullException("DbConnection is not found in the configuration file.");

            services.AddDbContext<CartContext>((sp, options) =>
            {
                options.UseNpgsql(connection, opt =>
                {
                    opt.SetPostgresVersion(new Version(16, 4));
                    opt.EnableRetryOnFailure(3);
                    opt.MigrationsAssembly(typeof(CartContext).Assembly.FullName!);
                });
                options.UseQueryTrackingBehavior(QueryTrackingBehavior.TrackAll);
                options.EnableSensitiveDataLogging();
                options.EnableDetailedErrors();
                //options.UseLoggerFactory(LoggerFactory.Create(builder => builder.));
            });

            return services;
        }
    }
}
