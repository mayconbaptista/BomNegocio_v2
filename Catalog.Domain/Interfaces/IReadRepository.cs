
using BuildBlocks.Domain.Abstractions;
using System.Linq.Expressions;

namespace Catalog.Domain.Interfaces
{
    public interface IReadRepository<TModel, Tkey> 
        where TModel : BaseEntity<Tkey> 
        where Tkey : notnull, IEquatable<Tkey>
    {
        public Task<TModel?> GetByIdAsync(Tkey id);
        public TModel? GetById (Tkey id);
        public Task<IList<TModel>> GetAllAsync(Expression<Func<TModel, bool>>? expression = null, bool asTraking = false);
        public Task<TModel?> FindAsync(Expression<Func<TModel, bool>> expression, bool asTraking = false);
        public TModel? Find (Expression<Func<TModel, bool>> expression, bool asTraking = true);
    }
}
