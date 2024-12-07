using System;

namespace BuildBlocks.Domain.Models
{
    public abstract class BaseModel<TKey> where TKey : notnull, IEquatable<TKey>
    {
        public TKey Id { get; set; }
    }
}
