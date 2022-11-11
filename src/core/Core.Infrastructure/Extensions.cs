using Core.Domain.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Npgsql;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using StackExchange.Redis;
using System.Reflection;

namespace Core.Infrastructure
{
    public static class Extensions
    {
        public static IEnumerable<IDomainEvent> GetDomainEvents(this DbContext dbContext)
        {
            var entities = dbContext.ChangeTracker.Entries<IEntityRootBase>()
                .Where(e => e.Entity.DomainEvents != null && e.Entity.DomainEvents.Any());

            return entities
                .SelectMany(x => x.Entity.DomainEvents)
                .ToList();
        }

        public static void AddDistributedTracing(this IServiceCollection services, IConfiguration configuration, DependencyOptions dependencyOptions)
        {
            ArgumentNullException.ThrowIfNull(services, nameof(services));

            string appName = configuration.GetValue<string>("Tracing:AppName");
            string assemblyVersion = Assembly.GetExecutingAssembly().GetName().Version?.ToString() ?? "unknown";
            var resourceBuilder = ResourceBuilder.CreateDefault().AddService(appName, serviceVersion: assemblyVersion, serviceInstanceId: Environment.MachineName);

            services.AddOpenTelemetryTracing((options) => {
                options.SetResourceBuilder(resourceBuilder)
                     .AddHttpClientInstrumentation(options =>
                     {
                         options.RecordException = true;
                     })
                    .AddAspNetCoreInstrumentation(options =>
                    {
                        options.RecordException = true;
                    })
                .AddNpgsql()
                .AddSource("MassTransit");

                if (dependencyOptions.AddRedis)
                {
                    options.Configure((sp, builder) =>
                     {
                         RedisCache cache = (RedisCache)sp.GetRequiredService<IDistributedCache>();
                         builder.AddRedisInstrumentation(cache.GetConnection());
                     });
                }

                options.AddOtlpExporter(otlpOptions =>
                {
                    otlpOptions.Endpoint = new Uri(configuration.GetValue<string>("Tracing:OtlpEndpoint"));
                });
            });

            
        }

        public static ConnectionMultiplexer GetConnection(this RedisCache cache)
        {
            //ensure connection is established
            typeof(RedisCache).InvokeMember("Connect", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.InvokeMethod, null, cache, new object[] { });

            //get connection multiplexer
            var fi = typeof(RedisCache).GetField("_connection", BindingFlags.Instance | BindingFlags.NonPublic);
            var connection = (ConnectionMultiplexer)fi.GetValue(cache);
            return connection;
        }
    }
}
