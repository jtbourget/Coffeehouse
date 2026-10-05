using System;

namespace Coffeehouse.Api.Models.DTOs;

public class VideoResponseDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string VideoUrl { get; set; } = string.Empty;
    public DateTime UploadedAt { get; set; }
    public int CandidateProfileId { get; set; }
}
