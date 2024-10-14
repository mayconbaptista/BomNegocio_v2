using Classifields.Domain.Account;

namespace Classifields.Infra.Data.Identity
{
    public class AuthService(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager) : IAuthenticate
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly SignInManager<ApplicationUser> _signInManager = signInManager;

        public async Task<bool> AuthenticateAsync(string email, string password)
        {
            var result = await _signInManager.PasswordSignInAsync(email, password, false, lockoutOnFailure: false);

            return result.Succeeded;
        }

        public async Task LogOut()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<bool> RegisterUserAsync(string email, string password)
        {

            var AppUser = new ApplicationUser { UserName = email, Email = email };

            var result = await _userManager.CreateAsync(AppUser, password);

            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(AppUser, false);
            }

            return result.Succeeded;
        }

        ValueTask<bool> IAuthenticate.AuthenticateAsync(string email, string password)
        {
            throw new NotImplementedException();
        }

        ValueTask<bool> IAuthenticate.RegisterUserAsync(string email, string password)
        {
            throw new NotImplementedException();
        }
    }
}
