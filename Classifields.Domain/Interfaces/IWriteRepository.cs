using Classifields.Domain.Entities;

namespace Classifields.Domain.Interfaces;

public interface IWriteRepository<TEntity> : IReadRepository<TEntity> where TEntity : BaseEntity
{
    Task<TEntity> CreateAsync(TEntity entity);
    TEntity Update(TEntity entity);
    TEntity Delete(TEntity entity);
}
