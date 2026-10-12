namespace Coffeehouse.Api.DTOs;

/// <summary>
/// Data transfer object representing a candidate profile video.
/// </summary>
public class VideoResponseDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string VideoUrl { get; set; } = string.Empty;
    public DateTime UploadedAt { get; set; }
    public int CandidateProfileId { get; set; }
}
