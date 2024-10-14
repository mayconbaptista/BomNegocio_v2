namespace Classifields.Domain.Account;

public interface ISeedUserRole
{
    ValueTask<bool> AddUserToRole(string email, string roleName);
    ValueTask<bool> AddRole(string roleName);
}
