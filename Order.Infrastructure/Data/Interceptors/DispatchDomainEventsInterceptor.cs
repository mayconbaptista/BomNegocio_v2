using BuildBlocks.Domain.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using MediatR;

namespace Order.Infrastructure.Data.Interceptors
{
    public class DispatchDomainEventsInterceptor(IMediator mediator) : SaveChangesInterceptor
    {
        private readonly IMediator _mediator = mediator;

        public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
        {
            this.DispatchEventsAsync(eventData.Context).GetAwaiter().GetResult();

            return base.SavingChanges(eventData, result);
        }

        public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
        {
            await this.DispatchEventsAsync(eventData.Context);

            return await base.SavingChangesAsync(eventData, result, cancellationToken);
        }

        public async Task DispatchEventsAsync(DbContext? context)
        {
            if (context is null) return;

            var domainEntities = context.ChangeTracker
                .Entries<BaseAuditableEntity>()
                .Where(po => po.Entity.Events.Any())
                .Select(po => po.Entity);

            var domainEvents = domainEntities
                .SelectMany(po => po.Events)
                .ToArray();

            foreach (var entity in domainEntities)
            {
                entity.ClearDomainEvents();
            }

            foreach (var domainEvent in domainEvents)
            {
                await this._mediator.Publish(domainEvent);
            }
        }
    }
}
