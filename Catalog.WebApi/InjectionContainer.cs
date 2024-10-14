
namespace Catalog.WebApi
{
    public static class InjectionContainer
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {

            var connection = configuration.GetConnectionString("CatalogConnection3") ?? throw new ArgumentNullException("Connection string not found");

            services.AddDbContext<CatalogContext>(options =>
            {
                options.UseNpgsql(connection, opt =>
                {
                    opt.SetPostgresVersion(new Version(16, 4));
                    //opt.EnableRetryOnFailure(3);
                });
                //options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
                options.EnableSensitiveDataLogging();
                options.EnableDetailedErrors();
                //options.UseLoggerFactory(LoggerFactory.Create(builder => builder.AddConsole()));
            });

            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IImageRepository, ImageRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped<IProductService, ProductService>();

            return services;
        }
    }
}
