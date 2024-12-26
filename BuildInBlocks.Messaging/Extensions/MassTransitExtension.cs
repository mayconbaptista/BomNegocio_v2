﻿
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using MassTransit;

namespace BuildInBlocks.Messaging.Extensions
{
    public static class MassTransitExtension
    {
        public static IServiceCollection AddMassTransit(this IServiceCollection services, IConfiguration configuration, Assembly? assemby = null)
        {
            services.AddMassTransit(config =>
            {
                config.SetKebabCaseEndpointNameFormatter();

                if (assemby != null)
                {
                    config.AddConsumers(assemby);
                }

                config.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(new Uri(configuration["MessageBroker:Host"]!), h =>
                    {
                        h.Username(configuration["MessageBroker:Username"]!);
                        h.Password(configuration["MessageBroker:Password"]!);
                    });

                    cfg.ConfigureEndpoints(context);
                });
            });

            return services;
        }
    }
}
