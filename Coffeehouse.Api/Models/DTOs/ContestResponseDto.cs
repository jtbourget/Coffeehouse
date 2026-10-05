namespace Coffeehouse.Api.Models.DTOs;

public class ContestResponseDto
{
    public int Id { get; set; }
    public string OfficeName { get; set; } = string.Empty;
    public string OcdId { get; set; } = string.Empty;
    public List<CandidateResponseDto> Candidates { get; set; } = new();
}
