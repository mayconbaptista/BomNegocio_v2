
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Order.Infrastructure.Payment.Configurations;
using Order.Infrastructure.Payment.Dtos;
using System.Net.Http.Headers;

namespace Order.Infrastructure.Payment.Handlers
{
    internal class AuthDelegatingHandler(IMemoryCache memoryCache,  HttpClient oauthHttpClient, IOptions<PaymentConfig> options) : DelegatingHandler
    {
        private IMemoryCache _memoryCache = memoryCache;
        private readonly HttpClient _oauthHttpClient = oauthHttpClient;
        private readonly PaymentConfig _options = options.Value;

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var token = await Token();
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            return await base.SendAsync(request, cancellationToken);
        }

        private async ValueTask<string> Token()
        {
            if (_memoryCache.TryGetValue<string>("EFI_access_token", out string acessTokenKey) && string.IsNullOrEmpty(acessTokenKey))
                return acessTokenKey!;

            using var request = new HttpRequestMessage(HttpMethod.Post, this._options.BaseUrl + this._options.EndPoints.OauthToken);
            request.Headers.Authorization = new AuthenticationHeaderValue("Basic", this._options.Credentials.GetBasicToken());
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                { "grant_type", "client_credentials" }
            });

            using var response = await this._oauthHttpClient.SendAsync(request);

            response.EnsureSuccessStatusCode();

            var tokenResponse = await response.Content.ReadAsStringAsync();

            var tokenDto = System.Text.Json.JsonSerializer.Deserialize<PaymentTokenDto>(tokenResponse);

            this._memoryCache.Set("EFI_access_token", tokenDto!.AccessToken, DateTime.Now.AddMinutes(TimeSpan.FromSeconds(tokenDto.ExpiresIn).Minutes));

            return tokenDto!.AccessToken;
        }
    }
}
