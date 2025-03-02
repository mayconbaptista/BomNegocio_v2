

using Catalog.Api.Data.Interfaces;
using Catalog.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Api.Data.Repositories
{
    public sealed class ProductRepository(CatalogContext catalogContext)
        : WriteRepository<ProductEntity, Guid>(catalogContext), IProductRepository
    {
        public async Task<ICollection<ProductEntity>> GetByCategoryAsync(Guid categoryId)
        {
            var query = _context.Products
                .Where(x => x.CategoryId == categoryId)
                .AsNoTracking();

            return await query.OrderBy(c => c.Name).ToListAsync();
        }

        public async Task<ICollection<ProductEntity>> GetRange (List<Guid> productIds)
        {
            var query = _context.Products
                .Where(x => productIds.Contains(x.Id))
                .Select(x => new ProductEntity
                {
                    Id = x.Id,
                    SkuCode = x.SkuCode,
                    Name = x.Name,
                    Price = x.Price,
                    Quantity = x.Quantity,
                })
                .AsNoTracking();

            return await query.OrderBy(c => c.Name).ToListAsync();
        }
    }
}
