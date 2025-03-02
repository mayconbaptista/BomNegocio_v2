using System;

namespace BuildBlocks.Domain.Abstractions
{
    public abstract class BaseEntity<TKey> where TKey : notnull, IEquatable<TKey>
    {
        public TKey Id { get; set; }
    }
}
