using System.Net;
using System.Text;
using System.Text.Json;
using Coffeehouse.Models;
using Microsoft.Extensions.Logging;

namespace Coffeehouse.Services;

public class AuthService : IAuthService
{
    private readonly HttpClient _httpClient;
    private readonly ITokenStore _tokenStore;
    private readonly ILogger<AuthService> _logger;
    private readonly JsonSerializerOptions _jsonOptions = new(JsonSerializerDefaults.Web);

    public AuthService(HttpClient httpClient, ITokenStore tokenStore, ILogger<AuthService> logger)
    {
        _httpClient = httpClient;
        _tokenStore = tokenStore;
        _logger = logger;
    }

    public async Task<AuthResult> LoginAsync(string username, string password)
    {
        _logger.LogInformation("Attempting login for user: {Username}", username);

        try
        {
            var payload = new { username, password };
            var json = JsonSerializer.Serialize(payload, _jsonOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("api/auth/login", content);

            if (response.IsSuccessStatusCode)
            {
                var body = await response.Content.ReadAsStringAsync();
                var loginResponse = JsonSerializer.Deserialize<LoginResponseDto>(body, _jsonOptions);

                if (loginResponse != null && !string.IsNullOrEmpty(loginResponse.AccessToken))
                {
                    await _tokenStore.SaveTokenAsync(loginResponse.AccessToken, loginResponse.ExpiresIn);
                    _logger.LogInformation("Login successful for user: {Username}", username);
                    return AuthResult.Success();
                }

                return AuthResult.Failed("Invalid response from server.");
            }

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                _logger.LogWarning("Login failed: wrong credentials for user: {Username}", username);
                return AuthResult.Failed("Wrong user name or password.");
            }

            if (response.StatusCode == (HttpStatusCode)429)
            {
                _logger.LogWarning("Login rate limited for user: {Username}", username);
                return AuthResult.Failed("Too many attempts. Wait a minute and try again.");
            }

            return AuthResult.Failed($"Login failed (status {(int)response.StatusCode}).");
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Network failure reaching login API.");
            return AuthResult.Failed("Can't reach the API. Is it running?");
        }
        catch (OperationCanceledException ex)
        {
            _logger.LogError(ex, "Timeout reaching login API.");
            return AuthResult.Failed("Can't reach the API. Is it running?");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error during login.");
            return AuthResult.Failed("An unexpected error occurred. Please try again.");
        }
    }

    public async Task LogoutAsync()
    {
        _logger.LogInformation("Logging out user.");
        await _tokenStore.ClearTokenAsync();
    }

    public async Task<bool> IsLoggedInAsync()
    {
        return await _tokenStore.HasValidTokenAsync();
    }
}
