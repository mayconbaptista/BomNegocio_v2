
namespace Order.Infrastructure.Payment.Configurations
{
    internal record PaymentConfig
    {
            public string BaseUrl { get; init; }
            public PaymentEndPointsConfig EndPoints { get; init; }
            public PaymentCredentialsConfig Credentials { get; init; }
    }
}
