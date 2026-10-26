using Microsoft.Extensions.Logging;

namespace Coffeehouse.Services;

public class SecureTokenStore : ITokenStore
{
    private const string TokenKey = "auth_access_token";
    private const string ExpiryKey = "auth_token_expiry";

    private readonly ILogger<SecureTokenStore> _logger;
    private string? _inMemoryToken;
    private DateTime? _inMemoryExpiry;

    public SecureTokenStore(ILogger<SecureTokenStore> logger)
    {
        _logger = logger;
    }

    public async Task<string?> GetTokenAsync()
    {
        try
        {
            var token = await SecureStorage.Default.GetAsync(TokenKey) ?? _inMemoryToken;
            var expiryStr = await SecureStorage.Default.GetAsync(ExpiryKey);

            DateTime expiry;
            if (!string.IsNullOrEmpty(expiryStr) && DateTime.TryParse(expiryStr, null, System.Globalization.DateTimeStyles.RoundtripKind, out var parsedExpiry))
            {
                expiry = parsedExpiry;
            }
            else if (_inMemoryExpiry.HasValue)
            {
                expiry = _inMemoryExpiry.Value;
            }
            else
            {
                // No expiry found or invalid
                await ClearTokenAsync();
                return null;
            }

            if (DateTime.UtcNow >= expiry)
            {
                _logger.LogInformation("Stored JWT token has expired. Clearing token.");
                await ClearTokenAsync();
                return null;
            }

            return token;
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "SecureStorage failed during GetTokenAsync; falling back to in-memory cache.");
            if (_inMemoryToken != null && _inMemoryExpiry.HasValue && DateTime.UtcNow < _inMemoryExpiry.Value)
            {
                return _inMemoryToken;
            }
            return null;
        }
    }

    public async Task SaveTokenAsync(string token, int expiresInSeconds)
    {
        // 30 second safety margin so token never expires in-flight
        var marginSeconds = Math.Max(0, expiresInSeconds - 30);
        var expiry = DateTime.UtcNow.AddSeconds(marginSeconds);
        var expiryStr = expiry.ToString("O");

        _inMemoryToken = token;
        _inMemoryExpiry = expiry;

        try
        {
            await SecureStorage.Default.SetAsync(TokenKey, token);
            await SecureStorage.Default.SetAsync(ExpiryKey, expiryStr);
            _logger.LogInformation("Saved JWT token to SecureStorage (expires {Expiry}).", expiryStr);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "SecureStorage failed during SaveTokenAsync; cached in memory only.");
        }
    }

    public async Task ClearTokenAsync()
    {
        _inMemoryToken = null;
        _inMemoryExpiry = null;

        try
        {
            SecureStorage.Default.Remove(TokenKey);
            SecureStorage.Default.Remove(ExpiryKey);
            _logger.LogInformation("Cleared JWT token from SecureStorage.");
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "SecureStorage failed during ClearTokenAsync.");
        }

        await Task.CompletedTask;
    }

    public async Task<bool> HasValidTokenAsync()
    {
        var token = await GetTokenAsync();
        return !string.IsNullOrEmpty(token);
    }
}
