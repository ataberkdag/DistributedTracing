using Core.Domain.Abstractions;
using Messages;

namespace Core.Infrastructure.Services.Interfaces
{
    public interface IIntegrationEventBuilder
    {
        IntegrationEvent GetIntegrationEvent(IDomainEvent domainEvent);
        string GetQueueName(IDomainEvent domainEvent);
    }
}
