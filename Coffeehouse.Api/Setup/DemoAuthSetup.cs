using Coffeehouse.Api.Authentication;
using Microsoft.AspNetCore.Identity;

namespace Coffeehouse.Api.Setup;

public static class DemoAuthSetup
{
    public static async Task<int> RunAsync(string projectPath)
    {
        Console.WriteLine("Setting up demo authentication user.");

        var username = PromptUntilValid("Username: ", s => !string.IsNullOrWhiteSpace(s) && s.Length <= 100);
        var password = PromptUntilValid("Password (min 8 chars): ", s => s.Length >= 8);
        var confirm = SecretInput.ReadPassword("Confirm password: ");

        if (password != confirm)
        {
            Console.WriteLine("Passwords do not match.");
            return 1;
        }

        var hasher = new PasswordHasher<DemoUser>();
        var userId = Guid.NewGuid();
        var dummyUser = new DemoUser { Id = userId, Username = username, PasswordHash = string.Empty };
        var hash = hasher.HashPassword(dummyUser, password);

        var key = JwtTokenService.GenerateRandomSigningKey();

        var values = new Dictionary<string, object>
        {
            ["Authentication:SigningKey"] = key,
            ["Authentication:DemoUsers:Users:0:Id"] = userId,
            ["Authentication:DemoUsers:Users:0:Username"] = username,
            ["Authentication:DemoUsers:Users:0:PasswordHash"] = hash,
        };

        await UserSecretsWriter.SetAsync(projectPath, values);

        Console.WriteLine("Demo user and signing key saved to user secrets.");
        return 0;
    }

    private static string PromptUntilValid(string prompt, Func<string, bool> validate)
    {
        while (true)
        {
            var value = SecretInput.ReadPassword(prompt);
            if (validate(value)) return value;
            Console.WriteLine("Invalid input; try again.");
        }
    }
}
