namespace BuildBlocks.Domain.Abstractions
{
    public abstract class BaseAuditableEntity<TKey> : BaseEntity<TKey>
        where TKey : notnull, IEquatable<TKey>
    {
        public DateTimeOffset CreateAt { get; protected init; }
        public DateTimeOffset? LastModifiedAt { get; protected set; }
    }
}
