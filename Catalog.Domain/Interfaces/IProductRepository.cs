
namespace Catalog.Domain.Interfaces
{
    public interface IProductRepository : IWriteRepository<ProductModel>
    {
        public Task<IEnumerable<ProductModel>> GetByCategoryAsync(uint categoryId);
    }
}
