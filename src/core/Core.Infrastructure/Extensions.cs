using Core.Domain.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Npgsql;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
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

        public static void AddDistributedTracing(this IServiceCollection services, IConfiguration configuration)
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

                options.AddOtlpExporter(otlpOptions =>
                {
                    otlpOptions.Endpoint = new Uri(configuration.GetValue<string>("Tracing:OtlpEndpoint"));
                });
            });
        }
    }
}
