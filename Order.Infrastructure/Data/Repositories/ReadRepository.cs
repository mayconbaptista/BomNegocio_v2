using BuildBlocks.Domain.Abstractions;
using Microsoft.EntityFrameworkCore;
using Order.Domain.Interfaces;
using System.Linq.Expressions;

namespace Order.Infrastructure.Data.Repositories
{
    internal abstract class ReadRepository<TModel>(OrderContext orderContext) 
        : IReadRepository<TModel> where TModel : BaseAuditableEntity
    {

        private readonly OrderContext _orderContext = orderContext;
        protected readonly DbSet<TModel> _dbSet = orderContext.Set<TModel>();

        public TModel? Find(Expression<Func<TModel, bool>> expression, bool asTraking = true)
        {
            throw new NotImplementedException();
        }

        public Task<TModel?> FindAsync(Expression<Func<TModel, bool>> expression, bool asTraking = false)
        {
            throw new NotImplementedException();
        }

        public Task<IList<TModel>> GetAllAsync(Expression<Func<TModel, bool>>? expression = null, bool asTraking = false)
        {
            throw new NotImplementedException();
        }

        public TModel? GetById(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<TModel?> GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
