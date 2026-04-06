
namespace Order.Infrastructure.Payment.Configurations
{
    internal record PaymentCredentialsConfig
    {
        public string ClientId { get; init; }
        public string ClientSecret { get; init; }

        public string GetBasicToken()
        {
            var token = $"{ClientId}:{ClientSecret}";
            var tokenBytes = System.Text.Encoding.UTF8.GetBytes(token);
            return Convert.ToBase64String(tokenBytes);
        }
    }
}
