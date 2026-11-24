
using System.Text.Json.Serialization;

namespace Order.Infrastructure.Payment.Dtos
{
    internal record PaymentTokenDto
    {
        [JsonPropertyName("access_token")]
        required public string AccessToken { get; init; }
        [JsonPropertyName("token_type")]
        required public string TokenType { get; init; }
        [JsonPropertyName("expires_in")]
        required public uint ExpiresIn { get; init; }
        [JsonPropertyName("scope")]
        public string Scope { get; init; }
    }
}
