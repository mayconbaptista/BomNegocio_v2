using BuildInBlocks.Messaging.Abstractions;
using BuildInBlocks.Messaging.Dtos;

namespace BuildInBlocks.Messaging.Events
{
    public record CartCheckoutEvent : IntegrationEvent
    {
        public CustomerDto Customer { get; init; }

        public AddressDto ShippingAddress { get; init; }

        public AddressDto BillingAddress { get; init; }

        public List<CartItemDto> Itens { get; init; } = new();
    }
}
