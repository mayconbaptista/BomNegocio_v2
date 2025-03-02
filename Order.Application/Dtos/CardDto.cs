
namespace Order.Application.Dtos;

public record CardDto (string CardNumber,string HolderName, string CardExpiration, string Cvv, int Type);
