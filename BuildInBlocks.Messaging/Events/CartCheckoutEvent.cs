
using BuildInBlocks.Messaging.Abstractions;
using BuildInBlocks.Messaging.Models;

namespace BuildInBlocks.Messaging.Events
{
    public record CartCheckoutEvent : IntegrationEvent
    {
        public Guid CustomerId { get; init; }
        public string UserName { get; init; }

        public string City { get; init; }
        public string Street { get; init; }
        public string State { get; init; }
        public string Country { get; init; }
        public string ZipCode { get; init; }

        public string CardNumber { get; init; }
        public string CardHolderName { get; init; }
        public string CardExpiration { get; init; }
        public string CardSecurityNumber { get; init; }
        public int CardTypeId { get; init; }

        public string Buyer { get; init; }
        public decimal Total { get; init; }
        public string Currency { get; init; }
        public List<CartItem> Items { get; init; }
    }
}
