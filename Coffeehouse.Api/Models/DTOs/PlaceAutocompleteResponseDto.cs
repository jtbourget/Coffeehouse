namespace Coffeehouse.Api.Models.DTOs;

/// <summary>
/// Data transfer object representing an address autocomplete prediction from a places search.
/// </summary>
public class PlaceAutocompleteResponseDto
{
    public string PlaceId { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string MainText { get; set; } = string.Empty;
    public string SecondaryText { get; set; } = string.Empty;
}
