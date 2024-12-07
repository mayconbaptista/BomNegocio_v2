using Auth.WebApi.Models;

namespace Auth.WebApi.Services.Interfaces
{
    public interface IAuthService
    {
        Task<TokenModel> LoginAsync(LoginModel model);
        Task<string> RegisterAsync(UserCreateDTo model);
    }
}
