using BuildBlocks.Domain.ValueObjects;
using BuildInBlocks.Messaging.Events;
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

            await this._sender.Send(command);
        }

        public CreateOrderCommand MapToCreateOrderCommand(CartCheckoutEvent cartCheckoutEvent)
        {
            var shippingAddress = new AddressDto(
                cartCheckoutEvent.ShippingAddressName,
                cartCheckoutEvent.ShippingAddressStreet,
                cartCheckoutEvent.ShippingAddressCity,
                cartCheckoutEvent.ShippingAddressState,
                cartCheckoutEvent.ShippingAddressCountry,
                cartCheckoutEvent.ShippingAddressZipCode);

            var billingAddress = new AddressDto(
                cartCheckoutEvent.BillingAddressName,
                cartCheckoutEvent.BillingAddressStreet,
                cartCheckoutEvent.BillingAddressCity,
                cartCheckoutEvent.BillingAddressState,
                cartCheckoutEvent.BillingAddressCountry,
                cartCheckoutEvent.BillingAddressZipCode);


            var customer = new CustomerDto(
                cartCheckoutEvent.CustomerId,
                cartCheckoutEvent.CustomerName,
                cartCheckoutEvent.CustomerEmail);

            var createOrderCommand = new CreateOrderCommand
            {
                ShippingAdress = shippingAddress,
                BillingAdress = billingAddress,
                Customer = customer,
                OrderItems = cartCheckoutEvent.OrderItems.Select(i => new OrderItemDto(i.ProductId, i.ProductName, i.PriceUni, i.Quantity)).ToList()
            };

            return createOrderCommand;
        }
    }
}
