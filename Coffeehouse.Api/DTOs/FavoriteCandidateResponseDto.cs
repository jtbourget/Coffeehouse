namespace Coffeehouse.Api.DTOs;

/// <summary>
/// Data transfer object representing a user's favorited candidate.
/// </summary>
public class FavoriteCandidateResponseDto
{
    public int Id { get; set; }
    public int ContestId { get; set; }
    public int CandidateId { get; set; }
    public string OfficeName { get; set; } = string.Empty;
    public string ContestOfficeName { get; set; } = string.Empty;
    public string CandidateName { get; set; } = string.Empty;
    public string Party { get; set; } = string.Empty;
}
