using Core.Infrastructure.Persistence;
using Order.Domain.Abstractions;
using Order.Infrastructure.Persistence.Context;

namespace Order.Infrastructure.Persistence.Repositories
{
    public class OrderRepository : Repository<Order.Domain.Entities.Order>, IOrderRepository
    {
        public OrderRepository(OrderDbContext context) : base(context)
        {

        }
    }
}
