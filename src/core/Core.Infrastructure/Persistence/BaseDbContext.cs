using Microsoft.EntityFrameworkCore;

namespace Core.Infrastructure.Persistence
{
    public class BaseDbContext : DbContext
    {

        public BaseDbContext(DbContextOptions opt) : base(opt)
        {

        }
    }
}
