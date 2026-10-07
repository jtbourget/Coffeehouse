namespace Coffeehouse.Api.Models.DTOs;

/// <summary>
/// Data transfer object representing a candidate running in an election contest.
/// </summary>
public class CandidateResponseDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Party { get; set; } = string.Empty;
    public string PhotoUrl { get; set; } = string.Empty;
    public string CampaignWebsite { get; set; } = string.Empty;
    public string Bio { get; set; } = string.Empty;
}
