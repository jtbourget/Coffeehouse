namespace Coffeehouse.Api.DTOs;

/// <summary>
/// Data transfer object for saving or updating a user's address.
/// Notice that Id is not present as clients cannot set the database-generated key.
/// </summary>
public class SaveAddressRequestDto
{
    public string Street { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
    public string ZipCode { get; set; } = string.Empty;
}
