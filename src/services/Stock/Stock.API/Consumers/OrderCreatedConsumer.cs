using MassTransit;
using Messages.Events;

namespace Stock.API.Consumers
{
    public class OrderCreatedConsumer : IConsumer<OrderCreatedIE>
    {
        public Task Consume(ConsumeContext<OrderCreatedIE> context)
        {
            Console.WriteLine($"Message Consumed | {context.Message.CorrelationId}");

            return Task.CompletedTask;
        }
    }
}
