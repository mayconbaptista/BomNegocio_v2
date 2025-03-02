
using MediatR;

namespace BuildBlocks.Domain.Abstractions
{
    public abstract class BaseEvent : INotification
    {
        public Guid EventId { get; } = Guid.NewGuid();

        public DateTimeOffset OccurredOn { get; } = DateTimeOffset.UtcNow;

        public string EventType => GetType().AssemblyQualifiedName!;
    }
}
