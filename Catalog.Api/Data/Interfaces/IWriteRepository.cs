
using BuildBlocks.Domain.Abstractions;

namespace Catalog.Api.Data.Interfaces
{
    public interface IWriteRepository<TModel, Tkey> : IReadRepository<TModel, Tkey>
        where TModel : BaseEntity<Tkey>
        where Tkey : notnull, IEquatable<Tkey>
    {
        public Task<TModel> AddAsync(TModel model);
        public void Update(TModel model);
        public void Delete(TModel model);
    }
}
