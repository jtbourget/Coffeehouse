namespace Coffeehouse.Models;

/// <summary>
/// Represents an address autocomplete suggestion returned from a places search.
/// </summary>
public class AddressSuggestion
{
    public string PlaceId { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string MainText { get; set; } = string.Empty;
    public string SecondaryText { get; set; } = string.Empty;
}
