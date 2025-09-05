using Amazon.S3;
using Catalog.Api.Configuration;
using Catalog.Api.Data;
using Catalog.Api.Data.Interfaces;
using Catalog.Api.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

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
                options.UseMySql(connection, new MySqlServerVersion(new Version(8,0,37)), opt =>
                {
                    opt.EnableRetryOnFailure(3);
                    opt.MigrationsAssembly(typeof(CatalogContext).Assembly.FullName!);
                });
                options.UseQueryTrackingBehavior(QueryTrackingBehavior.TrackAll);
                options.EnableSensitiveDataLogging();
                options.EnableDetailedErrors();
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
