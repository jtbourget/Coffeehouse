namespace Coffeehouse.Api.Models.DTOs;

/// <summary>
/// Data transfer object for saving a video associated with a candidate profile.
/// </summary>
public class SaveVideoRequestDto
{
    public string Title { get; set; } = string.Empty;
    public string VideoUrl { get; set; } = string.Empty;
}
