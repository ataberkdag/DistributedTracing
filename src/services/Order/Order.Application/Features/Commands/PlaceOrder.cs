using Core.Application.Common;
using MediatR;
using Order.Application.Models;
using Order.Domain.Abstractions;
using Order.Domain.Dtos;
using System.Text;
using System.Text.Json;

namespace Order.Application.Features.Commands
{
    public static class PlaceOrder
    {
        public class Command : IRequest<BaseResult<Response>>
        {
            public Guid UserId { get; set; }
            public List<OrderItemDto> OrderItems { get; set; }

        }

        public class CommandHandler : IRequestHandler<Command, BaseResult<Response>>
        {
            private readonly IOrderUnitOfWork _uow;
            private readonly IHttpClientFactory _httpClientFactory;

            public CommandHandler(IOrderUnitOfWork uow, IHttpClientFactory httpClientFactory)
            {
                _uow = uow;
                _httpClientFactory = httpClientFactory;
            }

            public async Task<BaseResult<Response>> Handle(Command request, CancellationToken cancellationToken)
            {
                await ReportRequest();

                var order = Order.Domain.Entities.Order.CreateOrder(request.UserId, request.OrderItems);

                await _uow.Orders.AddAsync(order);

                await _uow.SaveChangesAsync();

                return BaseResult<Response>.Success(new Response { CorrelationId = Guid.NewGuid()});
            }

            // Distributed Tracing Test
            private async Task ReportRequest()
            {
                var addReportRequest = JsonSerializer.Serialize(new AddReportRequest("PlaceOrder"));
                var message = new HttpRequestMessage(HttpMethod.Post, "http://localhost:5203/api/Reports")
                {
                    Content = new StringContent(addReportRequest, Encoding.UTF8, "application/json")
                };

                try
                {
                    var client = _httpClientFactory.CreateClient();

                    await client.SendAsync(message);
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
        }

        public class Response
        {
            public Guid CorrelationId { get; set; }
        }
    }
}
