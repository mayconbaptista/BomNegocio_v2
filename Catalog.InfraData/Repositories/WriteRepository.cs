
using BuildBlocks.Domain.Abstractions;

namespace Catalog.InfraData.Repositories
{
    public abstract class WriteRepository<TModel, Tkey> (CatalogContext catalogContext) 
        : ReadRepository<TModel, Tkey>(catalogContext), IWriteRepository<TModel,Tkey> 
        where TModel : BaseEntity<Tkey>
        where Tkey : notnull, IEquatable<Tkey>
    {
        public async Task<TModel> AddAsync(TModel model)
        {
            await this._dbSet.AddAsync(model);

            return model;
        }

        public void Delete(TModel entt)
        {
            this._dbSet.Remove(entt);
        }

        public void Update(TModel model)
        {
            this._dbSet.Update(model);
        }
    }
}
