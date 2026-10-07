namespace Coffeehouse.Api.Models.DTOs;

/// <summary>
/// Data transfer object representing a user's address.
/// </summary>
public class AddressResponseDto
{
    public int Id { get; set; }
    public string Street { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
    public string ZipCode { get; set; } = string.Empty;
}
