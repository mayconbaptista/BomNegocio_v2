using Amazon.S3;
using Catalog.Api.Configuration;
using Catalog.Api.Data;
using Catalog.Api.Data.Interfaces;
using Catalog.Api.Data.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Api
{
    public static class DependencyInjection
    {

        public static IServiceCollection AddApiServices (this IServiceCollection services, IConfiguration configuration)
        {
            var connection = configuration.GetConnectionString("DefaultConnection") 
                ?? throw new ArgumentNullException("Error ao obter a string de conecção.");

            services.AddDbContext<CatalogContext>((sp, options) =>
            {
                options.UseNpgsql(connection, opt =>
                {
                    opt.SetPostgresVersion(new Version(16, 4));
                    opt.EnableRetryOnFailure(3);
                    opt.MigrationsAssembly(typeof(CatalogContext).Assembly.FullName!);
                });
                options.UseQueryTrackingBehavior(QueryTrackingBehavior.TrackAll);
                options.EnableSensitiveDataLogging();
                options.EnableDetailedErrors();
                //options.UseLoggerFactory(LoggerFactory.Create(builder => builder.));
            });

            services.AddDefaultAWSOptions(configuration.GetAWSOptions("AWS"));
            services.AddAWSService<IAmazonS3>(configuration.GetAWSOptions("AWS:S3"), ServiceLifetime.Singleton);

            services.AddHttpClient();

            services.Configure<AwsServiceS3Config>(configuration.GetSection("ProductConfig:AwsServiceS3Config"));

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IImageRepository, ImageRepository>();

            return services;
        }
    }
}
