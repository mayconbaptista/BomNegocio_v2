using BuildBlocks.Domain.Abstractions.CQRS;
using BuildInBlocks.Messaging.Dtos;
using BuildInBlocks.Messaging.Events;
using Catalog.Api;
using Grpc.Core;
using MassTransit;

namespace Cart.WebApi.Cart.CartCheckout;

public record CartCheckoutCommand : ICommand<CartCheckoutResult>
{
    public Guid CustomerId { get; init; }
    public AddressDto ShippingAddress { get; init; }
    public AddressDto BillingAddress { get; init; }
}

public record CartCheckoutResult(bool IsSuccess, string? ErrorMessage = null);

public class CartCheckoutHandler(
        IUnitOfWork unitOfWork, 
        IPublishEndpoint publish, 
        ProductGRPC.ProductGRPCClient client, 
        ILogger<CartCheckoutHandler> logger) 
    : ICommandHandler<CartCheckoutCommand, CartCheckoutResult>
{
    public async Task<CartCheckoutResult> Handle(CartCheckoutCommand request, CancellationToken _cancellationToken)
    {
        try
        {

            var cart = await unitOfWork.CartRepository.GetCartAsync(request.CustomerId);

            if (cart is null || cart?.Items.Count == 0)
            {
                throw new InvalidOperationException("O carrinho está vazio");
            }

            using var call = client.GetProducts(cancellationToken: _cancellationToken);

            // envia um strem de produtosRequest para o serviço de catálogo
            var sendTask = Task.Run(async () =>
            {
                foreach (var item in cart!.Items)
                {
                    var request = new ProductRequest
                    {
                        Id = item.ProductId.ToString(),
                        Quantity = item.Quantity
                    };

                    await call.RequestStream.WriteAsync(request);
                }
                await call.RequestStream.CompleteAsync();

            }, _cancellationToken);

            var productsResponse = new List<ProductReply>();

            // recebe um stream de produtosResponse do serviço de catálogo
            var responseTask = Task.Run(async () =>
            {
                await foreach (var response in call.ResponseStream.ReadAllAsync())
                {
                    productsResponse.Add(response);
                }
            }, _cancellationToken);

            await Task.WhenAll(sendTask, responseTask);

            if (cart!.Items.Count != productsResponse.Count)
            {
                throw new Exception("Erro ao obter produtos");
            }

            unitOfWork.CartRepository.Clear(cart!.Items);
            await unitOfWork.SaveChangesAsync(_cancellationToken);

            var cartCheckoutEvent = new CartCheckoutEvent
            {
                Customer = new CustomerDto(request.CustomerId, "User name", "maycon@ufu.com.br"),
                ShippingAddress = request.ShippingAddress,
                BillingAddress = request.BillingAddress,
                Itens = productsResponse.Select(x => new CartItemDto(Guid.Parse(x.Id), (decimal) x.Price, x.Quantity)).ToList()
            };

            await publish.Publish(cartCheckoutEvent, _cancellationToken);

            return new CartCheckoutResult(true);
        
        }
        catch(Exception ex)
        {
            logger.LogError(ex, $"{ex.Message}");
            throw;
        }
    }
}
