namespace Coffeehouse.Api.Models;

public class CandidateProfile
{
    public int Id { get; set; }
    
    /// <summary>
    /// The Open Civic Data Identifier (e.g., "ocd-division/country:us/state:ca")
    /// to link this profile to a specific geographic district.
    /// </summary>
    public string OcdId { get; set; } = string.Empty;
    
    /// <summary>
    /// The name of the candidate as it appears in the civic API, 
    /// used to match the third-party data to this claimed profile.
    /// </summary>
    public string Name { get; set; } = string.Empty;
    
    public string? Bio { get; set; }
    
    public string? WebsiteUrl { get; set; }
    
    // Navigation property for user-uploaded videos
    public List<Video> Videos { get; set; } = new();
}
