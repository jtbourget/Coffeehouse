using System.Text.Json;
using Microsoft.Extensions.Logging;

namespace Coffeehouse.Api.Services;

/// <summary>
/// Service for interacting with the Google Places API for address autocomplete and place details lookup.
/// </summary>
public class GooglePlacesService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;
    private readonly ILogger<GooglePlacesService> _logger;

    public GooglePlacesService(HttpClient httpClient, IConfiguration configuration, ILogger<GooglePlacesService> logger)
    {
        _httpClient = httpClient;
        _apiKey = configuration["GooglePlacesApiKey"] ?? string.Empty;
        _logger = logger;
    }

    public async Task<string> AutocompleteAsync(string query)
    {
        if (string.IsNullOrWhiteSpace(_apiKey))
        {
            _logger.LogWarning("Google Places API key is missing when requesting autocomplete for query '{Query}'.", query);
            throw new HttpRequestException("Google Places API key is missing. Please configure 'GooglePlacesApiKey'.", null, System.Net.HttpStatusCode.ServiceUnavailable);
        }

        _logger.LogDebug("Querying Google Places autocomplete for: {Query}", query);
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
        var content = await response.Content.ReadAsStringAsync();
        _logger.LogDebug("Google Places autocomplete succeeded for query '{Query}'.", query);
        return content;
    }

    public async Task<string> GetPlaceDetailsAsync(string placeId)
    {
        if (string.IsNullOrWhiteSpace(_apiKey))
        {
            _logger.LogWarning("Google Places API key is missing when requesting details for placeId '{PlaceId}'.", placeId);
            throw new HttpRequestException("Google Places API key is missing. Please configure 'GooglePlacesApiKey'.", null, System.Net.HttpStatusCode.ServiceUnavailable);
        }

        _logger.LogDebug("Querying Google Places details for placeId: {PlaceId}", placeId);
        var url = $"https://places.googleapis.com/v1/places/{placeId}?fields=addressComponents";
        
        var request = new HttpRequestMessage(HttpMethod.Get, url);
        request.Headers.Add("X-Goog-Api-Key", _apiKey);
        
        var response = await _httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        _logger.LogDebug("Google Places details succeeded for placeId '{PlaceId}'.", placeId);
        return content;
    }
}
