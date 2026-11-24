namespace Auth.WebApi.Models
{
    public class TokenModel
    {
        public string UserId { get; init; }
        public string AccessToken { get; init; }
        public DateTime ExpirationAt { get; init; }
    }
}
