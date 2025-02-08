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
        public readonly ISender _sender = sender;
        public readonly ILogger<CartCheckoutEventHandler> _logger = logger;

        public async Task Consume(ConsumeContext<CartCheckoutEvent> consumeContext)
        {
            this._logger.LogInformation("IntegrationEvent consumed: {0}", consumeContext.Message.GetType().Name);

            var command = this.MapToCreateOrderCommand(consumeContext.Message);

            //await this._sender.Send(command);
            await Task.Delay(1000);
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
