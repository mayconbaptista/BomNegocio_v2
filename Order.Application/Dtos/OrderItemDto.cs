

namespace Order.Application.Dtos;

public record OrderItemDto (Guid ProductId,Guid OrderId, decimal UnitPrice, int Quantity);
