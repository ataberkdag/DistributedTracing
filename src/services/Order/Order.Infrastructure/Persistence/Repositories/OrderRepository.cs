using Core.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Order.Domain.Abstractions;

namespace Order.Infrastructure.Persistence.Repositories
{
    public class OrderRepository : Repository<Order.Domain.Entities.Order>, IOrderRepository
    {
        public OrderRepository(DbContext context) : base(context)
        {

        }
    }
}
