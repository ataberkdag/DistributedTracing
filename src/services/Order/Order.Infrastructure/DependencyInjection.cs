﻿using Core.Infrastructure;
using Core.Infrastructure.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Order.Domain.Abstractions;
using Order.Infrastructure.Persistence.Context;
using Order.Infrastructure.Persistence.Repositories;
using Order.Infrastructure.Persistence.UnitOfWork;
using Order.Infrastructure.Services.Impl;

namespace Order.Infrastructure
{
    public static class DependencyInjection
    {
        public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            ArgumentNullException.ThrowIfNull(services, nameof(services));

            var dependencyOptions = new DependencyOptions
            {
                AddMessageBroker = true,
                MessageBrokerSettings = new MessageBrokerSettings
                {
                    ConnectionString = configuration.GetConnectionString("RabbitMQ"),
                    UserName = configuration["RabbitMq:UserName"],
                    Password = configuration["RabbitMq:Password"]
                }
            };

            services.AddCoreInfrastructure(dependencyOptions);

            services.AddSingleton<IIntegrationEventBuilder, OrderIntegrationEventBuilder>();
            services.AddDbContext<OrderDbContext>(x => x.UseNpgsql(configuration.GetConnectionString("Database")));

            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IOrderUnitOfWork, OrderUnitOfWork>();
        }
    }
}
