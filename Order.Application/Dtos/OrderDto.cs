
using BuildInBlocks.Messaging.Dtos;
using Order.Domain.ValueObjects;

namespace Order.Application.Dtos;

public record OrderDto
{
    public Guid Id { get; init; }
    public CustomerDto Customer { get; init; }
    public AddressDto ShippingAddress { get; init; }
    public AddressDto BillingAddress { get; init; }
    public decimal TotalPrice { get; init; }
    public List<OrderItemDto> Items { get; init; }
}
