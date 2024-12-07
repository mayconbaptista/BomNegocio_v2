
using System.Linq.Expressions;

namespace Catalog.Domain.Interfaces
{
    public interface IReadRepository<TModel> where TModel : BaseModel
    {
        public Task<TModel?> GetByIdAsync(int id);
        public TModel? GetById (int id);
        public Task<IList<TModel>> GetAllAsync(Expression<Func<TModel, bool>>? expression = null, bool asTraking = false);
        public Task<TModel?> FindAsync(Expression<Func<TModel, bool>> expression, bool asTraking = false);
        public TModel? Find (Expression<Func<TModel, bool>> expression, bool asTraking = true);
    }
}
