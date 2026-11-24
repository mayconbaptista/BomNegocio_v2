

using System.Text.Json.Serialization;

namespace Order.Infrastructure.Payment.Dtos;

internal record PaymentPixDto
{
    [JsonPropertyName("txid")]
    public string? TxId { get; init; }
    [JsonPropertyName("revisao")]
    public int? Revisao { get; init; }
    [JsonPropertyName("status")]
    public string? Status { get; init; }
    [JsonPropertyName("location")]
    public string? Location { get; init; }
    [JsonPropertyName("pixCopiaECola")]
    public string? PixCopiaECola { get; init; }
    [JsonPropertyName("calendario")]
    public CalendarioDto Calendario { get; init; }
    [JsonPropertyName("devedor")]
    public DevedorDto Devedor { get; init; }
    [JsonPropertyName("valor")]
    public ValorDto Valor { get; init; }
    [JsonPropertyName("chave")]
    public Guid Chave { get; init; }
    [JsonPropertyName("solicitacaoPagador")]
    public string SolicitacaoPagador { get; init; }
}
internal record CalendarioDto
{
    [JsonPropertyName("criacao")]
    public DateTime? Criacao { get; init; }
    [JsonPropertyName("expiracao")]
    public uint Expiracao { get; init; } = (uint) TimeSpan.FromMinutes(10).TotalMinutes;
}

internal record DevedorDto
{
    [JsonPropertyName("cpf")]
    public string Cpf { get; init; }
    [JsonPropertyName("nome")]
    public string Nome { get; init; }
}

internal record ValorDto
{
    [JsonPropertyName("original")]
    required public string Original { get; init; }
}
