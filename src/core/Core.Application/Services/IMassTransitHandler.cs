namespace Core.Application.Services
{
    public interface IMassTransitHandler
    {
        Task Publish(object @event);

        Task Send(string queueName, object @event);
    }
}
