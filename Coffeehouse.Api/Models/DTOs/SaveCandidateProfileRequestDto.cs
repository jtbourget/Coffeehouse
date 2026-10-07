namespace Coffeehouse.Api.Models.DTOs;

/// <summary>
/// Data transfer object for saving or updating a candidate profile.
/// </summary>
public class SaveCandidateProfileRequestDto
{
    public int Id { get; set; }
    public string OcdId { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? Bio { get; set; }
    public string? WebsiteUrl { get; set; }
}
