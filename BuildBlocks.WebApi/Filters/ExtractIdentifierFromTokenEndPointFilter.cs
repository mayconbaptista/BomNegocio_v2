using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Security.Claims;

namespace BuildBlocks.WebApi.Filters
{
    public class ExtractIdentifierFromTokenEndPointFilter(IConfiguration configuration) : IEndpointFilter
    {
        private readonly IConfiguration _configuration = configuration;
        private readonly string _fieldName = "NameIdentifier";

        public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
        {
            var httpContext = context.HttpContext;

            var authenticationConf = configuration.GetSection("Authentication:Schemes:Bearer")
                ?? throw new ArgumentNullException("Erro ao Obter a configuração de autenticação");

            var secretKey = authenticationConf.GetValue<string>("IssuerSigningKey")
                ?? throw new ArgumentNullException("Erro ao Obter a chave de autenticação do token jwt");

            if (string.IsNullOrEmpty(secretKey) || !httpContext.Request.Headers.TryGetValue("Authorization", out var authorizationHeader))
            {
                return Results.Unauthorized();
            }

            var token = authorizationHeader.ToString().Replace("Bearer ", "");

            var handler = new JwtSecurityTokenHandler();
            var tokenS = handler.ReadToken(token) as JwtSecurityToken;
            var secret = Encoding.UTF8.GetBytes(secretKey);
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(secret),
                ValidateIssuer = false,
                ValidateAudience = false
            };

            var principal = handler.ValidateToken(token, validationParameters, out var validatedToken);

            var claim = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

            if (claim is null)
            {
                return Results.Unauthorized();
            }

            httpContext.Items.Add(_fieldName, claim.Value);

            return await next(context);
        }
    }
}
