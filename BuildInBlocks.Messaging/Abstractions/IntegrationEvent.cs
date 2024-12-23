
namespace BuildInBlocks.Messaging.Abstractions
{
    public abstract record IntegrationEvent
    {
        public Guid Guid { get; } = Guid.NewGuid();
        public DateTimeOffset CreationAt { get; } = DateTime.UtcNow;
        public string EventType => GetType().AssemblyQualifiedName;
    }
}
