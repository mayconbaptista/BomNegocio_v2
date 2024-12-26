
using BuildBlocks.Domain.Abstractions;

namespace Order.Domain.Interfaces
{
    public interface IWriteRepository<TModel> : IReadRepository<TModel>
        where TModel : BaseAuditableEntity
    {
        public Task<TModel> AddAsync(TModel model);
        public void Delete(TModel model);
    }
}
