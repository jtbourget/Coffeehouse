namespace Coffeehouse.Api.DTOs;

/// <summary>
/// Data transfer object representing an election contest and its candidates.
/// </summary>
public class ContestResponseDto
{
    public int Id { get; set; }
    public int ElectionId { get; set; }
    public string OfficeName { get; set; } = string.Empty;
    public string OcdId { get; set; } = string.Empty;
    public List<CandidateResponseDto> Candidates { get; set; } = new();
}
