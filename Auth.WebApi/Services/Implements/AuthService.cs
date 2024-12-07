using Auth.WebApi.Models;
using Microsoft.AspNetCore.Identity;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Auth.WebApi.Services.Implements
{
    public sealed class AuthService : IAuthService
    {
        private readonly UserManager<UserModel> _userManager;
        private readonly SignInManager<UserModel> _signInManager;
        private readonly ITokenService _tokenService;
        private readonly IConfiguration _config;

        public AuthService(
                       UserManager<UserModel> userManager,
                       SignInManager<UserModel> signInManager,
                       ITokenService tokenService,
                       IConfiguration config)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
            _config = config;
        }

        public async Task<TokenModel> LoginAsync (LoginModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.UserEmail!);

            if (user is null || !await _userManager.CheckPasswordAsync(user, model.UserEmail + model.Password))
            {
                if (user is not null)
                {
                    user.AccessFailedCount++;
                    await _userManager.UpdateAsync(user);
                }

                throw new BusinesException(HttpStatusCode.BadRequest, "Usuário ou senha inválidos");
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.UserName!),
                new Claim(ClaimTypes.Email, user.Email!),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            var roles = await _userManager.GetRolesAsync(user);

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var accessToken = _tokenService.GenerateAccessToken(claims, this._config);

            await _userManager.UpdateAsync(user);

            var result = new TokenModel
            {
                UserId = user.Id,
                AcessToken = new JwtSecurityTokenHandler().WriteToken(accessToken),
                ExpirationAt = DateTime.Now.AddMinutes(5)
            };

            return result;
        }


        public async Task<string> RegisterAsync(UserCreateDTo userDto)
        {
            var userExists = await _userManager.FindByEmailAsync(userDto.Email);

            if (userExists != null)
                throw new BusinesException(HttpStatusCode.BadRequest, "Usuário já cadastrado");

            var user = new UserModel
            {
                UserName = userDto.UserName,
                Email = userDto.Email,
                EmailConfirmed = false,
                PhoneNumber = userDto.PhoneNumber,
                PhoneNumberConfirmed = false,
                TwoFactorEnabled = false,
                LockoutEnabled = false,
                AccessFailedCount = 0,
                SecurityStamp = Guid.NewGuid().ToString()
            };

            var result = await _userManager.CreateAsync(user, userDto.Email + userDto.Password);

            if (!result.Succeeded)
                throw new BusinesException(HttpStatusCode.BadRequest, result.ToString(),result.Errors.Select(e => e.Description).ToList());

            return user.Id;
        }
    }
}
