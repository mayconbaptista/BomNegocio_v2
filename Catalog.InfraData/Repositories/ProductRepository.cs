

namespace Catalog.InfraData.Repositories
{
    public sealed class ProductRepository(CatalogContext catalogContext)
        : WriteRepository<ProductModel>(catalogContext), IProductRepository
    {
        public async Task<IEnumerable<ProductModel>> GetByCategoryAsync(uint categoryId)
        {
            var query = _context.Products
                .Where(x => x.CategoryId == categoryId)
                .AsNoTracking();

            return await query.ToListAsync();
        }
    }
}
