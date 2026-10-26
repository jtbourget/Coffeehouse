namespace Coffeehouse.Api.Authentication;

public class DemoUser
{
    public required Guid Id { get; init; }
    public required string Username { get; init; }
    public required string PasswordHash { get; init; }
}
