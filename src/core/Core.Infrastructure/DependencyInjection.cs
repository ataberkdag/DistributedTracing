using Core.Application.Services;
using Core.Infrastructure.Services.Impl;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace Core.Infrastructure
{
    public static class DependencyInjection
    {
        public static async void AddCoreInfrastructure(this IServiceCollection services, IConfiguration configuration, DependencyOptions options)
        {
            ArgumentNullException.ThrowIfNull(services, nameof(services));
            ArgumentNullException.ThrowIfNull(options, nameof(options));

            if (options.AddHttpClient)
                services.AddHttpClient("DefaultHttpClient");

            if (options.AddMessageBroker)
            {
                services.AddMassTransit(options.MessageBrokerConfiguration);

                services.AddSingleton<IMassTransitHandler, MassTransitHandler>();
            }

            if (options.AddRedis)
            {
                services.AddStackExchangeRedisCache(options => { options.Configuration = configuration.GetConnectionString("Redis"); });
            }
                

            if (options.AddDistributedTracing)
                services.AddDistributedTracing(configuration, options);
        }
    }

    public class DependencyOptions
    {
        public bool AddHttpClient { get; set; }
        public bool AddDistributedTracing { get; set; }
        public bool AddRedis { get; set; }

        public bool AddMessageBroker { get; set; }
        public Action<IBusRegistrationConfigurator> MessageBrokerConfiguration { get; set; }

    }
}
