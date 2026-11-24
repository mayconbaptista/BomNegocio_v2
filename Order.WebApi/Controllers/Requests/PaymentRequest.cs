using System.Text.Json.Serialization;

namespace Order.WebApi.Controllers.Requests;

public record PaymentRequest
{
    [JsonPropertyName("pix")]
    required public PixData Pix { get; init; }
}

public record PixData
{
    [JsonPropertyName("endToEndId")]
    required public string EndToEndId { get; init; }
    [JsonPropertyName("txid")] 
    required public string Txid { get; init; }
    [JsonPropertyName("chave")] 
    required public string Chave { get; init; }
    [JsonPropertyName("valor")]
    public string Valor { get; init; }
    [JsonPropertyName("horario")]
    required public DateTime Horario { get; init; }
    [JsonPropertyName("infoPagador")] 
    public string? InfoPagador { get; init; }
    [JsonPropertyName("gnExtras")]
    public GnExtras? GnExtras { get; init; }
}

public record GnExtras(
    [property: JsonPropertyName("pagador")] Pagador? Pagador,
    [property: JsonPropertyName("split")] Split? Split,
    [property: JsonPropertyName("tarifa")] string? Tarifa
);

public record Pagador(
    [property: JsonPropertyName("nome")] string? Nome,
    [property: JsonPropertyName("cpf")] string? Cpf,
    [property: JsonPropertyName("cnpj")] string? Cnpj,
    [property: JsonPropertyName("codigoBanco")] string? CodigoBanco,
    [property: JsonPropertyName("contaBanco")] ContaBanco? ContaBanco
);

public record ContaBanco(
    [property: JsonPropertyName("codigoBanco")] string CodigoBanco,
    [property: JsonPropertyName("agencia")] string Agencia,
    [property: JsonPropertyName("conta")] string Conta,
    [property: JsonPropertyName("tipoConta")] string TipoConta
);

public record Split(
    [property: JsonPropertyName("id")] string Id,
    [property: JsonPropertyName("revisao")] int Revisao
);
