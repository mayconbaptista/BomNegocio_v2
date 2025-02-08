using BuildInBlocks.Messaging.Abstractions;
using BuildInBlocks.Messaging.Dtos;

namespace BuildInBlocks.Messaging.Events
{
    public record CartCheckoutEvent : IntegrationEvent
    {
        //public Guid CustomerId { get; init; }
        //public string CustomerName { get; init; }
        //public string CustomerEmail { get; init; }

        public CustomerDto Customer { get; init; }

        //public string ShippingAddressName { get; init; }
        //public string ShippingAddressCity { get; init; }
        //public string ShippingAddressStreet { get; init; }
        //public string ShippingAddressState { get; init; }
        //public string ShippingAddressCountry { get; init; }
        //public string ShippingAddressZipCode { get; init; }

        public AddressDto ShippingAddress { get; init; }

        //public string BillingAddressName { get; init; }
        //public string BillingAddressCity { get; init; }
        //public string BillingAddressStreet { get; init; }
        //public string BillingAddressState { get; init; }
        //public string BillingAddressCountry { get; init; }
        //public string BillingAddressZipCode { get; init; }

        public AddressDto BillingAddress { get; init; }

        //public string CardNumber { get; init; }
        //public string CardHolderName { get; init; }
        //public string CardExpiration { get; init; }
        //public string CardCvv { get; init; }
        //public int CardTypeId { get; init; }

        //public string Buyer { get; init; }
        //public decimal Total { get; init; }
        //public string Currency { get; init; }
        public List<CartItemDto> Itens { get; init; } = new();
    }
}
