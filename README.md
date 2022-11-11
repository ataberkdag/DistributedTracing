# DistributedTracing

.Net 6 Distributed Tracing sample with PostgreSQL, HttpClient, MassTransit and Redis.

![alt text](https://github.com/ataberkdag/DistributedTracing/blob/main/images/Jaeger1.PNG?raw=true)

![alt text](https://github.com/ataberkdag/DistributedTracing/blob/main/images/Jaeger2.PNG?raw=true)

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

## Run with Docker (Does not include OrderAPI, StockAPI and ReportAPI)


```bash
docker-compose -f docker-compose.yml up -d
```

## Migration

To apply migrations follow this command on Package Manager Console for Order Microservice. (Set starting project to API and set default project to Infrastructure on Package Manager Console)

```bash
update-database
```
