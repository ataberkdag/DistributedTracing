using Core.Application.Common;
using MediatR;
using Order.Domain.Abstractions;

namespace Order.Application.Features.Queries
{
    public static class CheckOrderStatus
    {
        public class Query : IRequest<BaseResult<Response>>
        {
            public Guid CorrelationId { get; set; }
        }

        public class QueryHandler : IRequestHandler<Query, BaseResult<Response>>
        {
            private readonly IOrderUnitOfWork _uow;

            public QueryHandler(IOrderUnitOfWork uow)
            {
                _uow = uow;
            }

            public async Task<BaseResult<Response>> Handle(Query request, CancellationToken cancellationToken)
            {
                var order = (await _uow.Orders.FindByQuery(x => x.CorrelationId == request.CorrelationId)).FirstOrDefault();

                if (order == null)
                    return BaseResult<Response>.Fail("9999", "9999", new Response { 
                        OrderStatus = 0
                    });

                return BaseResult<Response>.Success(new Response { 
                    OrderStatus = order.Status.GetHashCode()
                });
            }
        }

        public class Response
        {
            public int OrderStatus { get; set; }
        }
    }
}
