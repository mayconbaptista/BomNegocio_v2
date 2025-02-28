using BuildBlocks.Domain.ValueObjects;
using BuildInBlocks.Messaging.Events;
using Mapster;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Order.Application.CQRS.Order.EventsHandlers.Integration
{
    public class CartCheckoutEventHandler(ISender sender, ILogger<CartCheckoutEventHandler> logger)
        : IConsumer<CartCheckoutEvent>
    {

        public async Task Consume(ConsumeContext<CartCheckoutEvent> consumeContext)
        {
            try
            {
                logger.LogInformation("IntegrationEvent consumed: {0}", consumeContext.Message.GetType().Name);

                var command = this.MapToCreateOrderCommand(consumeContext.Message);

                await sender.Send(command);
            }
            catch(Exception ex)
            {
                logger.LogError(ex, $"Error:{ex.Message}");
                throw;
            }
        }

        public CreateOrderCommand MapToCreateOrderCommand(CartCheckoutEvent cartCheckoutEvent)
        {
            return new CreateOrderCommand
            {
                BillingAdress = cartCheckoutEvent.BillingAddress,
                ShippingAdress = cartCheckoutEvent.ShippingAddress,
                Customer = cartCheckoutEvent.Customer,
                OrderItems = cartCheckoutEvent.Itens.Select(x => new OrderItemDto(x.ProductId, x.Price, x.Quantity)).ToList()
            };
        }
    }
}
