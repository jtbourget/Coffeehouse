namespace Coffeehouse.Api.DTOs;

/// <summary>
/// Data transfer object representing an address prediction from the autocomplete API.
/// </summary>
public class PlaceAutocompleteResponseDto
{
    public string Description { get; set; } = string.Empty;
    public string PlaceId { get; set; } = string.Empty;
}
