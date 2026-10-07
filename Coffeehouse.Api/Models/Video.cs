using System;

namespace Coffeehouse.Api.Models;

/// <summary>
/// Represents a video entity associated with a candidate profile.
/// </summary>
public class Video
{
    public int Id { get; set; }
    
    public string Title { get; set; } = string.Empty;
    
    public string VideoUrl { get; set; } = string.Empty;
    
    public DateTime UploadedAt { get; set; } = DateTime.UtcNow;
    
    public int CandidateProfileId { get; set; }
    public CandidateProfile? CandidateProfile { get; set; }
}
