using System.Text.Json;

namespace Coffeehouse.Api.Services;

/// <summary>
/// Service for interacting with the Google Places API for address autocomplete and place details lookup.
/// </summary>
public class GooglePlacesService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;

    public GooglePlacesService(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _apiKey = configuration["GooglePlacesApiKey"] ?? string.Empty;
    }

    public async Task<string> AutocompleteAsync(string query)
    {
        if (string.IsNullOrWhiteSpace(_apiKey))
        {
            throw new HttpRequestException("Google Places API key is missing. Please configure 'GooglePlacesApiKey'.", null, System.Net.HttpStatusCode.ServiceUnavailable);
        }

        var url = "https://places.googleapis.com/v1/places:autocomplete";
        
        var request = new HttpRequestMessage(HttpMethod.Post, url);
        request.Headers.Add("X-Goog-Api-Key", _apiKey);
        
        var body = new
        {
            input = query,
            includedRegionCodes = new[] { "us" }
        };
        
        request.Content = new StringContent(JsonSerializer.Serialize(body), System.Text.Encoding.UTF8, "application/json");
        
        var response = await _httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync();
    }

    public async Task<string> GetPlaceDetailsAsync(string placeId)
    {
        if (string.IsNullOrWhiteSpace(_apiKey))
        {
            throw new HttpRequestException("Google Places API key is missing. Please configure 'GooglePlacesApiKey'.", null, System.Net.HttpStatusCode.ServiceUnavailable);
        }

        var url = $"https://places.googleapis.com/v1/places/{placeId}?fields=addressComponents";
        
        var request = new HttpRequestMessage(HttpMethod.Get, url);
        request.Headers.Add("X-Goog-Api-Key", _apiKey);
        
        var response = await _httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync();
    }
}
