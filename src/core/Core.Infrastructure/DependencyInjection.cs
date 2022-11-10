using Core.Application.Services;
using Core.Infrastructure.Services.Impl;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Infrastructure
{
    public static class DependencyInjection
    {
        public static void AddCoreInfrastructure(this IServiceCollection services, DependencyOptions options)
        {
            ArgumentNullException.ThrowIfNull(services, nameof(services));
            ArgumentNullException.ThrowIfNull(options, nameof(options));

            if (options.AddHttpClient)
                services.AddHttpClient("DefaultHttpClient");

            if (options.AddMessageBroker)
            {
                services.AddMassTransit(_ => {
                    _.UsingRabbitMq((context, cfg) =>
                    {
                        cfg.Host(host: options.MessageBrokerSettings?.ConnectionString, h =>
                        {
                            h.Username(options.MessageBrokerSettings?.UserName);
                            h.Password(options.MessageBrokerSettings?.Password);
                        });

                    });
                });

                services.AddSingleton<IMassTransitHandler, MassTransitHandler>();
            }

        }
    }

    public class DependencyOptions
    {
        public bool AddHttpClient { get; set; }

        public bool AddMessageBroker { get; set; }
        public MessageBrokerSettings MessageBrokerSettings { get; set; }

    }

    public class MessageBrokerSettings
    {
        public string ConnectionString { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
