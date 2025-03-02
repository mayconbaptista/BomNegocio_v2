

namespace Order.Application.Dtos;

public record OrderItemDto (Guid ProductId, decimal UnitPrice, uint Quantity);
