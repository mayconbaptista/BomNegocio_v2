
using BuildBlocks.Domain.Abstractions;
using System.Linq.Expressions;

namespace Catalog.InfraData.Repositories
{
    public abstract class ReadRepository<TModel, Tkey> (CatalogContext context) : IReadRepository<TModel, Tkey> 
        where TModel : BaseEntity<Tkey>
        where Tkey : notnull, IEquatable<Tkey>
    {
        protected readonly CatalogContext _context = context;
        protected readonly DbSet<TModel> _dbSet = context.Set<TModel>();

        public TModel? Find(Expression<Func<TModel, bool>> expression, bool asTraking = true)
        {
            var query = this._dbSet.AsQueryable();

            if (expression != null)
            {
                query = query.Where(expression);
            }
            if(!asTraking)
            {
                query = query.AsNoTracking();
            }

            return query.FirstOrDefault();
        }

        public async Task<TModel?> FindAsync(Expression<Func<TModel, bool>> expression, bool asTraking = false)
        {
            var query = this._dbSet.AsQueryable();

            if (expression != null)
            {
                query = query.Where(expression);
            }
            if (!asTraking)
            {
                query = query.AsNoTracking();
            }
            return await query.FirstOrDefaultAsync();
        }

        public async Task<IList<TModel>> GetAllAsync(Expression<Func<TModel, bool>>? expression = null, bool asTraking = false)
        {
            var query = _dbSet.AsQueryable();

            if (expression != null)
            {
                query = query.Where(expression);
            }
            if (!asTraking)
            {
                query = query.AsNoTracking();
            }
            return await query.ToListAsync();
        }

        public TModel? GetById(Tkey id)
        {
            return _dbSet.SingleOrDefault(x => x.Equals(id));
        }

        public async Task<TModel?> GetByIdAsync(Tkey id)
        {
            return await _dbSet.SingleOrDefaultAsync(x => x.Equals(id));
        }
    }
}
