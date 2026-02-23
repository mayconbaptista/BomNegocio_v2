
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using MassTransit;
using Amazon.SQS;
using Amazon.SimpleNotificationService;

namespace BuildInBlocks.Messaging.Extensions
{
    public static class MassTransitExtension
    {

        public static IServiceCollection AddMessageBroker(this IServiceCollection services, IConfiguration configuration, Assembly? consumer = null)
        {
            var AWS_ACESS_KEY = configuration["AWS_ACCESS_KEY_ID"] 
                ?? throw new ArgumentNullException("Erro ao obter o AWS_ACCESS_KEY_ID.");
            
            var AWS_ACESS_PASS = configuration["AWS_SECRET_ACCESS_KEY"] 
                ?? throw new ArgumentNullException("Erro ao obter o AWS_SECRET_ACCESS_KEY.");
            
            var AWS_REGION = configuration["AWS_REGION"] 
                ?? throw new ArgumentNullException("Erro ao obter o AWS_REGION.");

            var AWS_SERVICE_URL = configuration["AWS:ServiceURL"]
                ?? throw new ArgumentNullException("Erro ao obter o AWS_ServiceURL.");

            services.AddMassTransit(x =>
            {
                x.SetKebabCaseEndpointNameFormatter();

                if(consumer != null)
                {
                    x.AddConsumers(consumer);
                }

                x.UsingAmazonSqs((context, cfg) =>
                {
                    cfg.Host(AWS_REGION?.Trim()!, h =>
                    {
                        h.AccessKey(AWS_ACESS_KEY?.Trim()!);
                        h.SecretKey(AWS_ACESS_PASS?.Trim()!);

                        var enviroment = configuration["ASPNETCORE_ENVIRONMENT"]
                            ?? throw new ArgumentNullException("Erro ao obter a variavel de ambiente");

                        if (enviroment != "Production")
                            h.Scope(enviroment!, true);

                        h.Config(new AmazonSQSConfig
                        {
                            ServiceURL = AWS_SERVICE_URL?.Trim()
                        });

                        h.Config(new AmazonSimpleNotificationServiceConfig
                        {
                            ServiceURL = AWS_SERVICE_URL?.Trim()
                        });
                    });

                    cfg.ConfigureEndpoints(context);
                });
            });

            return services;
        }
    }
}
