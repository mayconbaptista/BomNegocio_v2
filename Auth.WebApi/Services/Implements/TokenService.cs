using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Auth.WebApi.Services.Implements
{
    public class TokenService : ITokenService
    {
        public JwtSecurityToken GenerateAccessToken(IEnumerable<Claim> claims, IConfiguration config)
        {
            var authenticationConf = config.GetSection("Authentication:Schemes:Bearer") 
                ?? throw new ArgumentNullException("Erro ao Obter a configuração de autenticação");

            var secretKey = authenticationConf.GetValue<string>("IssuerSigningKey")
                ?? throw new ArgumentNullException("Erro ao Obter a chave de autenticação do token jwt");

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

            var token = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(authenticationConf.GetValue<double>("TokenLifeTimeInMinutes")),
                SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature),
                Issuer = authenticationConf.GetValue<string>("ValidIssuer"),
                Audience = authenticationConf.GetValue<string>("ValidAudience")
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            return tokenHandler.CreateJwtSecurityToken(token);
        }


        public ClaimsPrincipal GetPrincipalFromExpiredToken(string epireToken, IConfiguration config)
        {
            var authenticationConf = config.GetSection("Authentication:Schemes:Bearer")
                ?? throw new ArgumentNullException("Erro ao Obter a configuração de autenticação");

            var secretKey = authenticationConf.GetValue<string>("IssuerSigningKey")
                ?? throw new ArgumentNullException("Erro ao Obter a chave de autenticação do token jwt");

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = key
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var principal = tokenHandler.ValidateToken(epireToken, tokenValidationParameters, out SecurityToken securityToken);

            if (securityToken is not JwtSecurityToken jwtSecurityToken
                || jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new SecurityTokenException("Token inválido");
            }

            return principal;
        }
    }
}
