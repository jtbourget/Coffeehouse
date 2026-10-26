using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace Coffeehouse.Api.Authentication;

public class DemoUserStore
{
    private readonly PasswordHasher<DemoUser> _passwordHasher;
    private readonly Dictionary<string, DemoUser> _users;

    public DemoUserStore(PasswordHasher<DemoUser> passwordHasher, IOptions<DemoUserStoreOptions> options)
    {
        _passwordHasher = passwordHasher;
        _users = options.Value.Users?.ToDictionary(
            u => u.Username,
            u => u.ToUser(),
            StringComparer.OrdinalIgnoreCase)
            ?? new(StringComparer.OrdinalIgnoreCase);
    }

    public bool TryGetUser(string username, out DemoUser user)
    {
        return _users.TryGetValue(username, out user!);
    }

    public PasswordVerificationResult VerifyPassword(DemoUser user, string password)
    {
        return _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, password);
    }
}

public class DemoUserStoreOptions
{
    public List<DemoUserEntry>? Users { get; set; }
}

public class DemoUserEntry
{
    public required Guid Id { get; set; }
    public required string Username { get; set; }
    public required string PasswordHash { get; set; }

    public DemoUser ToUser() => new()
    {
        Id = Id,
        Username = Username,
        PasswordHash = PasswordHash,
    };
}
