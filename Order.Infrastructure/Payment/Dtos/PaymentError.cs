using System.Text.Json.Serialization;

namespace Order.Infrastructure.Payment.Dtos;

internal record PaymentError
{
    [JsonPropertyName("nome")]
    public string Nome { get; init; }
    [JsonPropertyName("menssagem")]
    public string Menssagem { get; init; }
    [JsonPropertyName("erros")]
    public List<ErrorDescription> Errors { get; init; }
}

internal record ErrorDescription
{
    [JsonPropertyName("chave")]
    required public string Chave { get; init; }
    [JsonPropertyName("caminho")]
    required public string Caminho { get; init; }
    [JsonPropertyName("mensagem")]
    required public string Mensagem { get; init; }
}