
using BuildBlocks.Domain.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Order.Domain.Entities;

namespace Order.Infrastructure.Data.Interceptors
{
    public class AuditableEntityInterceptor : SaveChangesInterceptor
    {
        public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
        {
            this.AuditEntity(eventData.Context);

            return base.SavingChanges(eventData, result);
        }

        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
        {
            this.AuditEntity(eventData.Context);

            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }

        public void AuditEntity(DbContext? context)
        {
            if(context is null) return;

            foreach (var entity in context.ChangeTracker.Entries<BaseAuditableEntity>())
            {

                var now = DateTime.UtcNow;

                switch (entity.State)
                {
                    case EntityState.Added:
                        entity.Entity.CreateAt = now;
                        entity.Entity.LastModifiedAt = null;
                        break;

                    case EntityState.Modified:
                        entity.Entity.LastModifiedAt = now;
                        break;
                }
            }

        }
    }
}
