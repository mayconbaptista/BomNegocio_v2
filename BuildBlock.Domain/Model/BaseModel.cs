using System;

namespace BuildBlock.Domain.Model
{
    public abstract class BaseModel<TKey> where TKey : notnull, IEquatable<TKey>
    {
        public TKey Id { get; set; }
    }
}
