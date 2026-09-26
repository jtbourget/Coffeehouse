using System;

namespace Coffeehouse.Models;

public class Video
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string VideoUrl { get; set; } = string.Empty;
    public DateTime UploadedAt { get; set; }
}
