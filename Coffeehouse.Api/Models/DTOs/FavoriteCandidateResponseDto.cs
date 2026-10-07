namespace Coffeehouse.Api.Models.DTOs;

/// <summary>
/// Data transfer object representing a favorited candidate for a contest.
/// </summary>
public class FavoriteCandidateResponseDto
{
    public int Id { get; set; }
    public int ContestId { get; set; }
    public string OfficeName { get; set; } = string.Empty;
    public int CandidateId { get; set; }
    public string CandidateName { get; set; } = string.Empty;
    public string Party { get; set; } = string.Empty;
}
