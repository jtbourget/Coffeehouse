using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using Coffeehouse.Api.Services;

namespace Coffeehouse.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PlacesController : ControllerBase
{
    private readonly GooglePlacesService _placesService;

    public PlacesController(GooglePlacesService placesService)
    {
        _placesService = placesService;
    }

    [HttpGet("autocomplete")]
    public async Task<IActionResult> Autocomplete([FromQuery] string query)
    {
        if (string.IsNullOrWhiteSpace(query))
            return BadRequest("Query is required.");

        var rawJson = await _placesService.AutocompleteAsync(query);
        
        using var document = JsonDocument.Parse(rawJson);
        if (!document.RootElement.TryGetProperty("suggestions", out var suggestions))
            return Ok(new List<object>());
            
        var results = new List<object>();
        foreach (var s in suggestions.EnumerateArray())
        {
            if (s.TryGetProperty("placePrediction", out var p))
            {
                results.Add(new
                {
                    PlaceId = p.GetProperty("placeId").GetString(),
                    Description = p.GetProperty("text").GetProperty("text").GetString(),
                    MainText = p.GetProperty("structuredFormat").GetProperty("mainText").GetProperty("text").GetString(),
                    SecondaryText = p.GetProperty("structuredFormat").GetProperty("secondaryText").GetProperty("text").GetString()
                });
            }
        }
        
        return Ok(results);
    }

    [HttpGet("details/{placeId}")]
    public async Task<IActionResult> GetDetails(string placeId)
    {
        var rawJson = await _placesService.GetPlaceDetailsAsync(placeId);
        
        using var document = JsonDocument.Parse(rawJson);
        if (!document.RootElement.TryGetProperty("addressComponents", out var components))
            return NotFound();

        var streetNumber = "";
        var route = "";
        var city = "";
        var state = "";
        var zip = "";

        foreach (var component in components.EnumerateArray())
        {
            var types = component.GetProperty("types").EnumerateArray().Select(t => t.GetString()).ToList();
            var shortName = component.GetProperty("shortText").GetString();
            var longName = component.GetProperty("longText").GetString();

            if (types.Contains("street_number")) streetNumber = shortName;
            if (types.Contains("route")) route = shortName;
            if (types.Contains("locality")) city = longName;
            if (types.Contains("administrative_area_level_1")) state = shortName;
            if (types.Contains("postal_code")) zip = shortName;
        }

        return Ok(new
        {
            Street = $"{streetNumber} {route}".Trim(),
            City = city,
            State = state,
            ZipCode = zip
        });
    }
}
