using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Auth.WebApi.Services.Interfaces
{
    public interface ITokenService
    {
        JwtSecurityToken GenerateAccessToken(IEnumerable<Claim> claims, IConfiguration conf);
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token, IConfiguration conf);
    }
}
