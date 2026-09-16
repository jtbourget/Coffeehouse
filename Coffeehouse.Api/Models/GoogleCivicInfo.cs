using System.Text.Json.Serialization;

namespace Coffeehouse.Api.Models;

public class GoogleCivicInfoResponse
{
    [JsonPropertyName("normalizedInput")]
    public SimpleAddress? NormalizedInput { get; set; }

    [JsonPropertyName("kind")]
    public string? Kind { get; set; }

    /// <summary>
    /// A dictionary where the key is the OCD ID (e.g., "ocd-division/country:us/state:ca")
    /// </summary>
    [JsonPropertyName("divisions")]
    public Dictionary<string, Division>? Divisions { get; set; }

    [JsonPropertyName("offices")]
    public List<Office>? Offices { get; set; }

    [JsonPropertyName("officials")]
    public List<Official>? Officials { get; set; }
}

public class SimpleAddress
{
    [JsonPropertyName("line1")]
    public string? Line1 { get; set; }

    [JsonPropertyName("city")]
    public string? City { get; set; }

    [JsonPropertyName("state")]
    public string? State { get; set; }

    [JsonPropertyName("zip")]
    public string? Zip { get; set; }
}

public class Division
{
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    /// <summary>
    /// List of indices in the offices array that apply to this division.
    /// </summary>
    [JsonPropertyName("officeIndices")]
    public List<int>? OfficeIndices { get; set; }
}

public class Office
{
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    /// <summary>
    /// The OCD ID of the division this office belongs to.
    /// </summary>
    [JsonPropertyName("divisionId")]
    public string? DivisionId { get; set; }

    [JsonPropertyName("levels")]
    public List<string>? Levels { get; set; }

    [JsonPropertyName("roles")]
    public List<string>? Roles { get; set; }

    /// <summary>
    /// List of indices in the officials array that hold this office.
    /// </summary>
    [JsonPropertyName("officialIndices")]
    public List<int>? OfficialIndices { get; set; }
}

public class Official
{
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("address")]
    public List<SimpleAddress>? Address { get; set; }

    [JsonPropertyName("party")]
    public string? Party { get; set; }

    [JsonPropertyName("phones")]
    public List<string>? Phones { get; set; }

    [JsonPropertyName("urls")]
    public List<string>? Urls { get; set; }

    [JsonPropertyName("photoUrl")]
    public string? PhotoUrl { get; set; }

    [JsonPropertyName("emails")]
    public List<string>? Emails { get; set; }

    [JsonPropertyName("channels")]
    public List<Channel>? Channels { get; set; }
}

public class Channel
{
    [JsonPropertyName("type")]
    public string? Type { get; set; }

    [JsonPropertyName("id")]
    public string? Id { get; set; }
}
