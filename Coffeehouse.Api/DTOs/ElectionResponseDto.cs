namespace Coffeehouse.Api.DTOs;

/// <summary>
/// Data transfer object representing an election, voting locations, and contests.
/// </summary>
public class ElectionResponseDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateTime ElectionDate { get; set; }
    public string VotingLocationName { get; set; } = string.Empty;
    public string VotingLocationAddress { get; set; } = string.Empty;
    public List<ContestResponseDto> Contests { get; set; } = new();
}
