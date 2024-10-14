
namespace Catalog.InfraData.Repositories
{
    public abstract class WriteRepository<TModel> (CatalogContext catalogContext) 
        : ReadRepository<TModel>(catalogContext), IWriteRepository<TModel> where TModel : BaseModel
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
