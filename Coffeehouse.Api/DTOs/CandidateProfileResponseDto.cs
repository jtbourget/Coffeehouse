namespace Coffeehouse.Api.DTOs;

/// <summary>
/// Data transfer object representing a candidate profile and associated media.
/// Contains server-controlled properties like Id, CreatedAt, and IsVerified.
/// </summary>
public class CandidateProfileResponseDto
{
    public int Id { get; set; }
    public string OcdId { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? Bio { get; set; }
    public string? WebsiteUrl { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool IsVerified { get; set; }
    public List<VideoResponseDto> Videos { get; set; } = new();
}
