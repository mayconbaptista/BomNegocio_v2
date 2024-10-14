
namespace Catalog.Application.Interfaces
{
    public interface IProductService 
    {
        public Task<ProductModel> Save (ProductModel product);

        public Task<ProductModel> GetById(uint id); 
    }
}
