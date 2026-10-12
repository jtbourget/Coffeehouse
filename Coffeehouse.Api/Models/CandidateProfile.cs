namespace Coffeehouse.Api.Models;

/// <summary>
/// Represents a candidate profile entity.
/// Contains server-controlled fields that the client cannot set:
/// - Id (database-generated identity)
/// - CreatedAt (server-assigned timestamp)
/// - IsVerified (server-controlled status flag)
/// </summary>
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

    /// <summary>
    /// Server-generated creation timestamp (client cannot set).
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Server-controlled verification flag (client cannot set).
    /// </summary>
    public bool IsVerified { get; set; } = false;
    
    // Navigation property for user-uploaded videos
    public List<Video> Videos { get; set; } = new();
}
