
namespace Catalog.Application.Services
{
    public class ProductService(IUnitOfWork unitOfWork) : IProductService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<ProductModel> GetById(uint id)
        {
            try
            {

                var product = await this._unitOfWork
                    .IProductRepository
                    .FindAsync(x => x.Id == id) ?? throw new Exception("Product not found");

                return product;

            }catch(Exception ex)
            {
                throw;
            }
        }

        public async Task<ProductModel> Save (ProductModel product)
        {
            product.Validate();

            await _unitOfWork.IProductRepository.AddAsync(product);
            await _unitOfWork.CommitAsync();

            return product;
        }
    }
}
