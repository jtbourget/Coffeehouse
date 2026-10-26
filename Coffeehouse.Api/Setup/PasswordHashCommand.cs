using Coffeehouse.Api.Authentication;
using Microsoft.AspNetCore.Identity;

namespace Coffeehouse.Api.Setup;

public static class PasswordHashCommand
{
    public static int Run()
    {
        var password = SecretInput.ReadPassword("Password to hash: ");
        var userId = Guid.NewGuid();
        var user = new DemoUser { Id = userId, Username = "demo", PasswordHash = string.Empty };
        var hash = new PasswordHasher<DemoUser>().HashPassword(user, password);

        Console.WriteLine($"Id: {userId}");
        Console.WriteLine($"PasswordHash: {hash}");
        return 0;
    }
}
