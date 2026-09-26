using System.Text.Json;
using System.Text.Json.Serialization;

namespace Coffeehouse.Api.Services;

public class GeocodioService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;

    public GeocodioService(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        // Check for GeocodioApiKey in user secrets / appsettings
        _apiKey = configuration["GeocodioApiKey"] ?? string.Empty;
    }

    public async Task<List<string>> GetOcdIdsForAddressAsync(string address)
    {
        var ocdIds = new List<string> { "ocd-division/country:us/state:wi" }; // Always include state-wide

        if (string.IsNullOrEmpty(_apiKey))
        {
            // If the user hasn't set the key yet, return just the state-wide districts
            return ocdIds;
        }

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
            }
        }
        catch (Exception)
        {
            // Fail gracefully
        }

        return ocdIds.Distinct().ToList();
    }
}

// Response Models
public class GeocodioResponse
{
    [JsonPropertyName("results")]
    public List<GeocodioResult>? Results { get; set; }
}

public class GeocodioResult
{
    [JsonPropertyName("fields")]
    public GeocodioFields? Fields { get; set; }
}

public class GeocodioFields
{
    [JsonPropertyName("congressional_districts")]
    public List<GeocodioDistrict>? CongressionalDistricts { get; set; }

    [JsonPropertyName("state_legislative_districts")]
    public GeocodioStateLeg? StateLegislativeDistricts { get; set; }
}

public class GeocodioStateLeg
{
    [JsonPropertyName("house")]
    public List<GeocodioDistrict>? House { get; set; }

    [JsonPropertyName("senate")]
    public List<GeocodioDistrict>? Senate { get; set; }
}

public class GeocodioDistrict
{
    [JsonPropertyName("district_number")]
    public object? DistrictNumberObj { get; set; }

    [JsonPropertyName("ocd_id")]
    public string? OcdId { get; set; }

    // District number can sometimes be an int or a string
    public string DistrictNumber => DistrictNumberObj?.ToString() ?? "1";
}
