using Catalog.Api.Data.Interfaces;
using Grpc.Core;
using System.Runtime.CompilerServices;

namespace Catalog.Api.Services
{
    public class ProductService(ILogger<ProductService> logger, IUnitOfWork unitOfWork) : ProductGRPC.ProductGRPCBase
    {
        private readonly ILogger<ProductService> _logger = logger;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public override async Task GetProducts(
            IAsyncStreamReader<ProductRequest> requestStream,
            IServerStreamWriter<ProductReply> responseStream,
            ServerCallContext context)
        {
            _logger.LogInformation("GetProducts method called");
            var list = new List<Guid>();

            await foreach (var request in requestStream.ReadAllAsync())
            {
                list.Add(Guid.Parse(request.Id));
            }

            var products = await _unitOfWork.IProductRepository.GetRange(list);


            foreach (var product in products)
            {
                var element = new ProductReply
                {
                    Id = product.Id.ToString(),
                    SkuCode = product.SkuCode,
                    Price = (float) product.Price,
                    Quantity = product.Quantity,
                };

                await responseStream.WriteAsync(element);
            }

            await Task.CompletedTask;
        }
    }
}
