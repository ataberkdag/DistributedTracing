using Core.Application.Services;
using Core.Domain.Abstractions;
using Core.Domain.Abstractions.Persistence;
using Core.Infrastructure.Services.Interfaces;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace Core.Infrastructure.Persistence
{
    public abstract class UnitOfWork : IUnitOfWork
    {
        private readonly DbContext _dbContext;
        private readonly IIntegrationEventBuilder _integrationEventBuilder;
        private readonly IMassTransitHandler _massTransitHandler;

        public UnitOfWork(DbContext dbContext,
            IIntegrationEventBuilder integrationEventBuilder,
            IMassTransitHandler massTransitHandler)
        {
            _dbContext = dbContext;
            _integrationEventBuilder = integrationEventBuilder;
            _massTransitHandler = massTransitHandler;
        }

        public void Dispose()
        {
            _dbContext.Dispose();
        }

        public async Task SaveChangesAsync()
        {
            var domainEvents = _dbContext.GetDomainEvents();

            await _dbContext.SaveChangesAsync();

            this.DispatchDomainEvents(domainEvents);
        }

        private void DispatchDomainEvents(IEnumerable<IDomainEvent> domainEvents)
        {
            if (domainEvents == null || domainEvents.Count() == 0)
                return;

            var tasks = domainEvents.Select(async (domainEvent) => {

                var integrationEvent = _integrationEventBuilder.GetIntegrationEvent(domainEvent);
                var queueName = _integrationEventBuilder.GetQueueName(domainEvent);

                await _massTransitHandler.Send(queueName, integrationEvent);

            });

            Task.WhenAll(tasks);
        }
    }
}
