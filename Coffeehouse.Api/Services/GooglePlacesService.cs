using System.Text.Json;

namespace Coffeehouse.Api.Services;

public class GooglePlacesService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;

    public GooglePlacesService(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _apiKey = configuration["GooglePlacesApiKey"] ?? throw new ArgumentNullException("GooglePlacesApiKey is missing.");
    }

    public async Task<string> AutocompleteAsync(string query)
    {
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
        var url = $"https://places.googleapis.com/v1/places/{placeId}?fields=addressComponents";
        
        var request = new HttpRequestMessage(HttpMethod.Get, url);
        request.Headers.Add("X-Goog-Api-Key", _apiKey);
        
        var response = await _httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync();
    }
}
