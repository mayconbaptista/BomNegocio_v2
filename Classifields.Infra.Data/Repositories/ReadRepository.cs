using Classifields.Domain.Entities;
using Classifields.Domain.Interfaces;
using Classifields.Infra.Data.Context;

namespace Classifields.Infra.Data.Repositories;

public abstract class ReadRepository<T> : IReadRepository<T> where T : BaseEntity
{
    protected readonly BNContext _bnContext;
    protected readonly DbSet<T> _dbSet;

    public ReadRepository(BNContext bnContext)
    {
        _bnContext = bnContext;
        _dbSet = bnContext.Set<T>();
    }

    public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>>? predicate = null, bool Tracking = true)
    {
        IQueryable<T> query = _dbSet;

        if (predicate != null)
        {
            query = query.Where(predicate);
        }

        if (!Tracking)
        {
            query = query.AsNoTracking();
        }

        return await query.ToListAsync();
    }

    public async Task<T?> FindAsync(Expression<Func<T, bool>>? predicate = null, bool Tracking = true)
    {
        IQueryable<T> query = _dbSet;

        if (predicate != null)
        {
            query = query.Where(predicate);
        }

        if (!Tracking)
        {
            query = query.AsNoTracking();
        }

        return await query.FirstOrDefaultAsync();
    }

    public async Task<T?> GetByIdAsync(uint id)
    {
        return await _dbSet.FindAsync(id);
    }
}
