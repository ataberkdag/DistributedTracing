using MassTransit;
using Messages.Events;
using Microsoft.Extensions.Caching.Distributed;

namespace Stock.API.Consumers
{
    public class OrderCreatedConsumer : IConsumer<OrderCreatedIE>
    {
        private readonly IDistributedCache _cache;

        public OrderCreatedConsumer(IDistributedCache cache)
        {
            _cache = cache;
        }

        public async Task Consume(ConsumeContext<OrderCreatedIE> context)
        {
            Console.WriteLine($"Message Consumed | {context.Message.UserId}");

            var userId = await _cache.GetStringAsync(context.Message.UserId.ToString());
            if (userId != null)
                throw new Exception($"User {context.Message.UserId} Locked");
            else
                await _cache.SetStringAsync(context.Message.UserId.ToString(), 
                    context.Message.UserId.ToString(), new DistributedCacheEntryOptions { AbsoluteExpiration = DateTime.Now.AddMinutes(5)});

            return;
        }
    }
}
