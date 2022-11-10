using Core.Domain.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace Core.Infrastructure
{
    public static class Extensions
    {
        public static IEnumerable<IDomainEvent> GetDomainEvents(this DbContext dbContext)
        {
            var entities = dbContext.ChangeTracker.Entries<IEntityRootBase>()
                .Where(e => e.Entity.DomainEvents != null && e.Entity.DomainEvents.Any());

            return entities
                .SelectMany(x => x.Entity.DomainEvents)
                .ToList();
        }
    }
}
