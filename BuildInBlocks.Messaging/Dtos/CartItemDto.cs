
namespace BuildInBlocks.Messaging.Dtos;

public record CartItemDto(Guid ProductId, decimal Price, uint Quantity);
