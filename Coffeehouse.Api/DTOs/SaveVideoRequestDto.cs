namespace Coffeehouse.Api.DTOs;

/// <summary>
/// Data transfer object for saving a video for a candidate profile.
/// </summary>
public class SaveVideoRequestDto
{
    public string Title { get; set; } = string.Empty;
    public string VideoUrl { get; set; } = string.Empty;
}
