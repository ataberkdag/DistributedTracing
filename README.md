# DistributedTracing
.Net 6 Distributed Tracing sample.

### Services

## OrderAPI

Calls Report.API and sends OrderPlaced event.

- CQRS implementation.
- Repository and UnitOfWork pattern implementations.
- RabbitMq with MassTransit.
- PostgreSQL

## StockAPI

Consumes OrderPlaced event. Checks over Redis.

- Redis
- RabbitMQ with MassTransit

### Docker Images

- postgres:14.2-alpine3.15
- rabbitmq:3-management
- redis:6.2
- jaegertracing/opentelemetry-all-in-one
