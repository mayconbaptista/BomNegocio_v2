
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
        // Long polling: o ReceiveMessage fica "pendurado" no SQS por ate 20 s
        // (maximo permitido) e retorna IMEDIATAMENTE quando chega mensagem.
        // O default do MassTransit e 3 s: fila vazia = ~20 requests/min por
        // loop de recepcao (~876 mil/mes, cobrados). Com 20 s caem para
        // ~3/min, sem aumentar a latencia de entrega.
        private const ushort SqsLongPollingSeconds = 20;

        public static IServiceCollection AddMessageBroker(this IServiceCollection services, IConfiguration configuration, Assembly? consumer = null)
        {
            var isMigration = Environment.GetCommandLineArgs()
                .Any(arg => arg.Contains("migrations", StringComparison.OrdinalIgnoreCase));

            if (isMigration)
            {
                services.AddMassTransit(x => { x.UsingInMemory(); });

                Console.WriteLine("APlicando mastransit em memoria para migration#####");

                return services;
            }

            var awsRegion = configuration["AWS_REGION"]
                ?? throw new InvalidOperationException("Erro ao obter o AWS_REGION.");
            var environment = configuration["ASPNETCORE_ENVIRONMENT"]
                ?? throw new InvalidOperationException("Erro ao obter a variavel de ambiente");
            var isDevelopment = environment.Equals("Development", StringComparison.OrdinalIgnoreCase);

            services.AddMassTransit(x =>
            {
                x.SetKebabCaseEndpointNameFormatter();

                if(consumer != null)
                {
                    x.AddConsumers(consumer);
                }

                // Aplica-se a cada endpoint criado pelo ConfigureEndpoints
                // (uma fila por consumer).
                x.AddConfigureEndpointsCallback((_, _, endpoint) =>
                {
                    if (endpoint is IAmazonSqsReceiveEndpointConfigurator sqs)
                    {
                        sqs.WaitTimeSeconds = SqsLongPollingSeconds;
                    }
                });

                x.UsingAmazonSqs((context, cfg) =>
                {
                    // Fila temporaria do proprio bus (request/response).
                    cfg.WaitTimeSeconds = SqsLongPollingSeconds;

                    cfg.Host(awsRegion.Trim(), h =>
                    {
                        if (!environment.Equals("Production", StringComparison.OrdinalIgnoreCase))
                        {
                            h.Scope(environment.ToLowerInvariant(), true);
                        }

                        // Somente o desenvolvimento usa credenciais fake e endpoint do LocalStack.
                        // Em staging/producao, o SDK resolve credenciais pela IAM Role da EC2.
                        if (isDevelopment)
                        {
                            var accessKey = configuration["AWS_ACCESS_KEY_ID"]
                                ?? throw new InvalidOperationException("Erro ao obter o AWS_ACCESS_KEY_ID.");
                            var secretKey = configuration["AWS_SECRET_ACCESS_KEY"]
                                ?? throw new InvalidOperationException("Erro ao obter o AWS_SECRET_ACCESS_KEY.");
                            var serviceUrl = configuration["AWS_ENDPOINT_URL"]
                                ?? throw new InvalidOperationException("Erro ao obter o AWS_ENDPOINT_URL.");

                            h.AccessKey(accessKey.Trim());
                            h.SecretKey(secretKey.Trim());
                            h.Config(new AmazonSQSConfig
                            {
                                ServiceURL = serviceUrl.Trim()
                            });

                            h.Config(new AmazonSimpleNotificationServiceConfig
                            {
                                ServiceURL = serviceUrl.Trim()
                            });
                        }
                    });

                    cfg.ConfigureEndpoints(context);
                });
            });

            return services;
        }
    }
}
