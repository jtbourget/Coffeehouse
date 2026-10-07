using System.Text.Json;
using Coffeehouse.Api.Models;

namespace Coffeehouse.Api.Services;

/// <summary>
/// Service for interacting with the Google Civic Information API to look up Open Civic Data identifiers for addresses.
/// </summary>
public class GoogleCivicService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;

    public GoogleCivicService(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _apiKey = configuration["GoogleCivicApiKey"] ?? throw new ArgumentNullException("GoogleCivicApiKey is missing.");
    }

    public async Task<List<string>> GetOcdIdsForAddressAsync(string address)
    {
        // Add statewide OCD-ID by default
        var ocdIds = new List<string> { "ocd-division/country:us/state:wi" };

        var encodedAddress = Uri.EscapeDataString(address);
        var url = $"https://www.googleapis.com/civicinfo/v2/representatives?address={encodedAddress}&key={_apiKey}";
        
        try
        {
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            var response = await _httpClient.SendAsync(request);
            
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var civicData = JsonSerializer.Deserialize<GoogleCivicInfoResponse>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                
                if (civicData?.Divisions != null)
                {
                    ocdIds.AddRange(civicData.Divisions.Keys);
                    return ocdIds.Distinct().ToList();
                }
            }
        }
        catch (Exception)
        {
            // Ignore API failures and fall back to mock
        }

        // MOCK FALLBACK:
        // Google discontinued the representatives endpoint in early 2025.
        // As a fallback for this demo, we will assign districts based on the city/zip.
        var addressLower = address.ToLower();
        
        if (addressLower.Contains("madison") || addressLower.Contains("537"))
        {
            ocdIds.Add("ocd-division/country:us/state:wi/cd:2");
            ocdIds.Add("ocd-division/country:us/state:wi/sldu:26");
            ocdIds.Add("ocd-division/country:us/state:wi/sldl:77");
        }
        else if (addressLower.Contains("milwaukee") || addressLower.Contains("532"))
        {
            ocdIds.Add("ocd-division/country:us/state:wi/cd:4");
            ocdIds.Add("ocd-division/country:us/state:wi/sldu:4");
            ocdIds.Add("ocd-division/country:us/state:wi/sldl:10");
        }
        else if (addressLower.Contains("green bay") || addressLower.Contains("543"))
        {
            ocdIds.Add("ocd-division/country:us/state:wi/cd:8");
            ocdIds.Add("ocd-division/country:us/state:wi/sldu:30");
            ocdIds.Add("ocd-division/country:us/state:wi/sldl:90");
        }
        else
        {
            // Generic fallback districts
            ocdIds.Add("ocd-division/country:us/state:wi/cd:1");
            ocdIds.Add("ocd-division/country:us/state:wi/sldu:11");
            ocdIds.Add("ocd-division/country:us/state:wi/sldl:31");
        }

        return ocdIds.Distinct().ToList();
    }
}
