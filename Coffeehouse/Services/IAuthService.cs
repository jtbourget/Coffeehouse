using Coffeehouse.Models;

namespace Coffeehouse.Services;

public interface IAuthService
{
    Task<AuthResult> LoginAsync(string username, string password);
    Task LogoutAsync();
    Task<bool> IsLoggedInAsync();
}
