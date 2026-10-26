namespace Coffeehouse.Services;

public interface ITokenStore
{
    Task<string?> GetTokenAsync();
    Task SaveTokenAsync(string token, int expiresInSeconds);
    Task ClearTokenAsync();
    Task<bool> HasValidTokenAsync();
}
