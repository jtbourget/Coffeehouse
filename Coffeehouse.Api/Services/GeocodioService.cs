using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Logging;

namespace Coffeehouse.Api.Services;

/// <summary>
/// Service for interacting with the Geocodio API to retrieve geographic and legislative district data.
/// </summary>
public class GeocodioService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;
    private readonly ILogger<GeocodioService> _logger;

    public GeocodioService(HttpClient httpClient, IConfiguration configuration, ILogger<GeocodioService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
        // Check for GeocodioApiKey in user secrets / appsettings
        _apiKey = configuration["GeocodioApiKey"] ?? string.Empty;
    }

    public async Task<List<string>> GetOcdIdsForAddressAsync(string address)
    {
        var ocdIds = new List<string> { "ocd-division/country:us/state:wi" }; // Always include state-wide

        if (string.IsNullOrEmpty(_apiKey))
        {
            _logger.LogInformation("No GeocodioApiKey configured. Returning default statewide OCD ID.");
            return ocdIds;
        }

        _logger.LogDebug("Querying Geocodio API for address: {Address}", address);
        var encodedAddress = Uri.EscapeDataString(address);
        // Request cd (Congressional) and stateleg (State Senate/Assembly)
        var url = $"https://api.geocod.io/v1.7/geocode?q={encodedAddress}&fields=cd,stateleg&api_key={_apiKey}";
        
        try
        {
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            var response = await _httpClient.SendAsync(request);
            
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var data = JsonSerializer.Deserialize<GeocodioResponse>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                
                if (data?.Results != null && data.Results.Count > 0)
                {
                    var fields = data.Results[0].Fields;
                    if (fields != null)
                    {
                        // Parse Congressional Districts
                        if (fields.CongressionalDistricts != null)
                        {
                            foreach (var cd in fields.CongressionalDistricts)
                            {
                                if (!string.IsNullOrEmpty(cd.OcdId)) ocdIds.Add(cd.OcdId);
                                else ocdIds.Add($"ocd-division/country:us/state:wi/cd:{cd.DistrictNumber}");
                            }
                        }

                        // Parse State Legislative Districts
                        if (fields.StateLegislativeDistricts != null)
                        {
                            if (fields.StateLegislativeDistricts.House != null)
                            {
                                foreach (var house in fields.StateLegislativeDistricts.House)
                                {
                                    if (!string.IsNullOrEmpty(house.OcdId)) ocdIds.Add(house.OcdId);
                                    else ocdIds.Add($"ocd-division/country:us/state:wi/sldl:{house.DistrictNumber}");
                                }
                            }
                            if (fields.StateLegislativeDistricts.Senate != null)
                            {
                                foreach (var senate in fields.StateLegislativeDistricts.Senate)
                                {
                                    if (!string.IsNullOrEmpty(senate.OcdId)) ocdIds.Add(senate.OcdId);
                                    else ocdIds.Add($"ocd-division/country:us/state:wi/sldu:{senate.DistrictNumber}");
                                }
                            }
                        }
                    }
                }

                _logger.LogInformation("Geocodio resolved {Count} OCD IDs for address: {Address}", ocdIds.Count, address);
            }
            else
            {
                _logger.LogWarning("Geocodio API returned status code {StatusCode} for address: {Address}", response.StatusCode, address);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception while querying Geocodio API for address: {Address}", address);
        }

        return ocdIds.Distinct().ToList();
    }
}

// Response Models
/// <summary>
/// Represents the root response returned by the Geocodio geocoding API.
/// </summary>
public class GeocodioResponse
{
    [JsonPropertyName("results")]
    public List<GeocodioResult>? Results { get; set; }
}

/// <summary>
/// Represents an individual geocoding result item from the Geocodio API response.
/// </summary>
public class GeocodioResult
{
    [JsonPropertyName("fields")]
    public GeocodioFields? Fields { get; set; }
}

/// <summary>
/// Represents the additional fields data within a Geocodio result, including legislative districts.
/// </summary>
public class GeocodioFields
{
    [JsonPropertyName("congressional_districts")]
    public List<GeocodioDistrict>? CongressionalDistricts { get; set; }

    [JsonPropertyName("state_legislative_districts")]
    public GeocodioStateLeg? StateLegislativeDistricts { get; set; }
}

/// <summary>
/// Represents the state legislative districts returned by the Geocodio API.
/// </summary>
public class GeocodioStateLeg
{
    [JsonPropertyName("house")]
    public List<GeocodioDistrict>? House { get; set; }

    [JsonPropertyName("senate")]
    public List<GeocodioDistrict>? Senate { get; set; }
}

/// <summary>
/// Represents a legislative district in the Geocodio API response.
/// </summary>
public class GeocodioDistrict
{
    [JsonPropertyName("district_number")]
    public object? DistrictNumberObj { get; set; }

    [JsonPropertyName("ocd_id")]
    public string? OcdId { get; set; }

    // District number can sometimes be an int or a string
    public string DistrictNumber => DistrictNumberObj?.ToString() ?? "1";
}
