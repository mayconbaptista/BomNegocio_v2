
using BuildBlocks.Domain.Abstractions;
using Order.Domain.Interfaces;

namespace Order.Infrastructure.Data.Repositories
{
    internal abstract class WriteRepository<TModel>(OrderContext catalogContext)
        : ReadRepository<TModel>(catalogContext), IWriteRepository<TModel>
        where TModel : BaseAuditableEntity
    {
        public async Task<TModel> AddAsync(TModel model)
        {
            await this._dbSet.AddAsync(model);

            return model;
        }

        public void Delete(TModel model)
        {
            this._dbSet.Remove(model);
        }
    }
}
