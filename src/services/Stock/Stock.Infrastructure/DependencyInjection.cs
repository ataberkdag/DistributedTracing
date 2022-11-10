using Core.Infrastructure;
using Core.Infrastructure.Services.Interfaces;
using MassTransit;
using Messages;
using Messages.Events;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Stock.Infrastructure
{
    public static class DependencyInjection
    {
        public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration, DependencyOptions dependencyOptions)
        {
            ArgumentNullException.ThrowIfNull(services, nameof(services));

            services.AddCoreInfrastructure(dependencyOptions);
        }
    }
}
