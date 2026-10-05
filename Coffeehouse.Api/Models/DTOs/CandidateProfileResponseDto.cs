namespace Coffeehouse.Api.Models.DTOs;

public class CandidateProfileResponseDto
{
    public int Id { get; set; }
    public string OcdId { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? Bio { get; set; }
    public string? WebsiteUrl { get; set; }
    public List<VideoResponseDto> Videos { get; set; } = new();
}
