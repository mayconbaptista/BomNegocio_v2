
using BuildBlocks.Domain.Abstractions;
using System.Linq.Expressions;

namespace Order.Domain.Interfaces
{
    public interface IReadRepository<TModel>
        where TModel : BaseAuditableEntity
    {
        public Task<TModel?> GetByIdAsync(Guid id);
        public TModel? GetById(Guid id);
        public Task<IList<TModel>> GetAllAsync(Expression<Func<TModel, bool>>? expression = null, bool asTraking = false);
        public Task<TModel?> FindAsync(Expression<Func<TModel, bool>> expression, bool asTraking = false);
        public TModel? Find(Expression<Func<TModel, bool>> expression, bool asTraking = true);
    }
}
