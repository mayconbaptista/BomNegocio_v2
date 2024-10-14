using Classifields.Domain.Account;

namespace Classifields.Infra.Data.Identity
{
    public sealed class SeedUserRole : ISeedUserRole
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public SeedUserRole(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public async ValueTask<bool> AddRole(string roleName)
        {
            if (_roleManager.RoleExistsAsync(roleName).Result)
                throw new Exception(roleName + $"A {roleName} já existe");

            IdentityRole role = new IdentityRole
            {
                Name = roleName,
                NormalizedName = roleName.ToUpper()
            };

            IdentityResult result = await _roleManager.CreateAsync(role);

            return result.Succeeded;
        }

        public async ValueTask<bool> AddUserToRole(string email, string roleName)
        {
            ApplicationUser user = await _userManager
                .FindByEmailAsync(email.ToUpper())
                ?? throw new Exception("User não encontrado");

            if (!_roleManager.RoleExistsAsync(roleName).Result)
                throw new Exception("Role não encontrada");

            IdentityResult result = await _userManager.AddToRoleAsync(user, roleName);

            return result.Succeeded;
        }

        public async Task<bool> RemoveUserToRole(string email, string roleName)
        {
            if (_roleManager.RoleExistsAsync(roleName).Result)
                throw new Exception("Role não encontrada");

            ApplicationUser user = await _userManager.FindByEmailAsync(email.ToUpper())
                ?? throw new Exception("User não encontrado");

            IdentityResult result = await _userManager.RemoveFromRoleAsync(user, roleName);

            return result.Succeeded;
        }
    }
}
