namespace Classifields.Domain.Account;

public interface IAuthenticate
{
    ValueTask<bool> AuthenticateAsync(string email, string password);
    ValueTask<bool> RegisterUserAsync(string email, string password);
    Task LogOut();
}
