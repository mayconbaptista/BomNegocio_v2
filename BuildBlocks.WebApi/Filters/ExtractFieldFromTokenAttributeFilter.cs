using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;


namespace BuildBlocks.WebApi.Filters
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false)]
    public class ExtractFieldFromTokenAttributeFilter(string fieldName, IConfiguration configuration) : Attribute, IAsyncActionFilter
    {
        private readonly string _fieldName = fieldName == default ? "NameIdentifier" : fieldName;
        private readonly IConfiguration _configuration = configuration;

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var httpContext = context.HttpContext;

            var secretKey = _configuration["Jwt:SecretKey"];

            if (string.IsNullOrEmpty(secretKey))
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            if (!httpContext.Request.Headers.TryGetValue("Authorization", out var authorizationHeader))
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            var token = authorizationHeader.ToString().Replace("Bearer ", "");

            try
            {
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
                var claim = principal.Claims.FirstOrDefault(c => c.Type == _fieldName);

                if (claim is null)
                {
                    context.Result = new UnauthorizedResult();
                    return;
                }

                httpContext.Items.Add(_fieldName, claim.Value);
                await next();
            }
            catch (Exception)
            {
                context.Result = new UnauthorizedResult();
            }
        }
    }
}
