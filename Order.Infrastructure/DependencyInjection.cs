
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Order.Application.Common.Interfaces;
using Order.Domain.Interfaces;
using Order.Infrastructure.Payment;
using Order.Infrastructure.Payment.Configurations;
using Order.Infrastructure.Payment.Handlers;
using System.Net.Mime;
using System.Security.Cryptography.X509Certificates;

namespace Order.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            #region "Persistence"
            var connection = configuration.GetConnectionString("DbConnection")
                ?? throw new ArgumentNullException("DbConnection is not found in the configuration file.");

            services.AddScoped<DispatchDomainEventsInterceptor>();
            services.AddScoped<AuditableEntityInterceptor>();
            services.AddScoped<ISaveChangesInterceptor, DispatchDomainEventsInterceptor>();
            services.AddScoped<ISaveChangesInterceptor, AuditableEntityInterceptor>();

            services.AddDbContext<OrderContext>((sp, options) =>
            {
                var interceptors = sp.GetServices<ISaveChangesInterceptor>().ToArray();

                options.AddInterceptors(interceptors);

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
            #endregion
            #region "Payment"
            // Payment Processor
            services.AddMemoryCache();
            services.Configure<PaymentConfig>(configuration.GetSection("EFI"));
            services.AddTransient<AuditDelegatingHandler>();
            services.AddTransient<AuthDelegatingHandler>();

            if (EF.IsDesignTime)
            {
                services.AddHttpClient();
            }
            else
            {
                var pathCertificado = configuration["EFI:CertificatePath"] ?? throw new ArgumentNullException("Payment:CertificatePath is not found in the configuration file.");
                var passwordCertificado = configuration["EFI:CertificatePassword"] ?? string.Empty; //throw new ArgumentNullException("Payment:CertificatePassword is not found in the configuration file.");
                var baseUrl = configuration["EFI:BaseUrl"] ?? throw new ArgumentNullException("Payment:BaseUrl is not found in the configuration file.");
                
                var certificado = new X509Certificate2(pathCertificado , passwordCertificado);

                services.AddHttpClient<AuthDelegatingHandler>(client =>
                {
                    client.BaseAddress = new Uri(baseUrl);
                    client.DefaultRequestHeaders.Add("Accept", MediaTypeNames.Application.Json);
                    client.Timeout = TimeSpan.FromSeconds(30);

                }).AddHttpMessageHandler<AuditDelegatingHandler>()
                    .ConfigurePrimaryHttpMessageHandler(() =>
                    {
                        var handler = new HttpClientHandler();
                        handler.ClientCertificates.Add(certificado);
                        return handler;
                    });

                services.AddHttpClient<IPaymentProcessor, PaymentProcessor>(client =>
                {
                    client.BaseAddress = new Uri(baseUrl);
                    client.DefaultRequestHeaders.Add("Accept", MediaTypeNames.Application.Json);
                    client.Timeout = TimeSpan.FromSeconds(30);

                }).AddHttpMessageHandler<AuditDelegatingHandler>()
                    .AddHttpMessageHandler<AuthDelegatingHandler>()
                    .ConfigurePrimaryHttpMessageHandler(() =>
                    {
                        var handler = new HttpClientHandler();
                        handler.ClientCertificates.Add(certificado);
                        return handler;
                    });

            }

            //services.AddScoped<IPaymentProcessor, PaymentProcessor>();
            #endregion

            return services;
        }
    }
}
