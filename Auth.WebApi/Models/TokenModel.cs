namespace Auth.WebApi.Models
{
    public class TokenModel
    {
        public string UserId { get; init; }
        public string AcessToken { get; init; }
        public DateTime ExpirationAt { get; init; }
    }
}
