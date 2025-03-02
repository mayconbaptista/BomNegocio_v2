using System.ComponentModel.DataAnnotations.Schema;

namespace BuildBlocks.Domain.Abstractions
{
    public abstract class BaseAuditableEntity : BaseEntity<Guid>
    {
        public DateTimeOffset CreateAt { get; set; }
        public DateTimeOffset? LastModifiedAt { get; set; }


        private readonly List<BaseEvent> _domainEvents = new();

        [NotMapped]
        public IReadOnlyCollection<BaseEvent> Events => _domainEvents.AsReadOnly();

        public void AddDomainEvent(BaseEvent domainEvent)
        {
            _domainEvents.Add(domainEvent);
        }

        public void RemoveDomainEvent(BaseEvent domainEvent)
        {
            _domainEvents.Remove(domainEvent);
        }

        public void ClearDomainEvents()
        {
            _domainEvents.Clear();
        }
    }
}
