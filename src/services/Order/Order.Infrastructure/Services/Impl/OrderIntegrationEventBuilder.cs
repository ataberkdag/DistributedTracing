﻿using Core.Domain.Abstractions;
using Core.Infrastructure.Services.Impl;
using Core.Infrastructure.Services.Interfaces;
using Messages;
using Messages.Events;
using Order.Domain.Events;

namespace Order.Infrastructure.Services.Impl
{
    public class OrderIntegrationEventBuilder : BaseIntegrationEventBuilder, IIntegrationEventBuilder
    {
        public override string GetQueueName(IDomainEvent domainEvent)
        {
            var result = string.Empty;

            if (domainEvent is OrderCreated)
                result = RabbitMqConsts.OrderCreatedQueueName;

            return result;
        }

        public override IntegrationEvent GetIntegrationEvent(IDomainEvent domainEvent)
        {
            var result = default(IntegrationEvent);

            if (domainEvent is OrderCreated oc)
            {
                result = new OrderCreatedIE(oc.CorrelationId, oc.UserId, oc.OrderItems.Select(oi => new OrderItemIEDto(oi.ItemId, oi.Quantity)).ToList());
            }

            return result;
        }
    }
}
