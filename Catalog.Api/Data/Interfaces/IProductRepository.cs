using Catalog.Api.Entities;

namespace Catalog.Api.Data.Interfaces
{
    public interface IProductRepository : IWriteRepository<ProductEntity, Guid>
    {
        public Task<ICollection<ProductEntity>> GetByCategoryAsync(Guid? categoryId);

        public Task<ICollection<ProductEntity>> GetRange(List<Guid> productIds);

        public Task<IList<ImageEntity>> GeImages();

        Task<ProductEntity?> GetByIdWithDetais(Guid id);
    }
}
