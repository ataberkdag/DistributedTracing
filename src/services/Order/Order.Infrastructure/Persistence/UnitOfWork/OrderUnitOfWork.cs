using Core.Application.Services;
using Core.Infrastructure.Services.Interfaces;
using Order.Domain.Abstractions;
using Order.Infrastructure.Persistence.Context;

namespace Order.Infrastructure.Persistence.UnitOfWork
{
    public class OrderUnitOfWork : Core.Infrastructure.Persistence.UnitOfWork, IOrderUnitOfWork
    {
        public OrderUnitOfWork(OrderDbContext dbContext,
            IIntegrationEventBuilder integrationEventBuilder,
            IServiceProvider serviceProvider,
            IMassTransitHandler massTransitHandler) 
            : base(dbContext, integrationEventBuilder, massTransitHandler)
        {
            Orders = (IOrderRepository)serviceProvider.GetService(typeof(IOrderRepository));
        }

        public IOrderRepository Orders { get; set; }
    }
}
