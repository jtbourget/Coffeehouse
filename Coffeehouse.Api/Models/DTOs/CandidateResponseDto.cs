namespace Coffeehouse.Api.Models.DTOs;

public class CandidateResponseDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Party { get; set; } = string.Empty;
    public string PhotoUrl { get; set; } = string.Empty;
    public string CampaignWebsite { get; set; } = string.Empty;
    public string Bio { get; set; } = string.Empty;
}
