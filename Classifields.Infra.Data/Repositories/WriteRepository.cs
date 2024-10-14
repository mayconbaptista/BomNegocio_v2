using Classifields.Domain.Entities;
using Classifields.Domain.Interfaces;
using Classifields.Infra.Data.Context;

namespace Classifields.Infra.Data.Repositories;

public abstract class WriteRepository<T>(BNContext dbContext) : ReadRepository<T>(dbContext), IWriteRepository<T> where T : BaseEntity
{
    public async Task<T> CreateAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
        return entity;
    }

    public T Delete(T entity)
    {
        _dbSet.Remove(entity);
        return entity;
    }

    public T Update(T entity)
    {
        _dbSet.Update(entity);
        return entity;
    }
}
