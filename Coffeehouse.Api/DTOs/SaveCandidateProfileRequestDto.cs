namespace Coffeehouse.Api.DTOs;

using System.ComponentModel.DataAnnotations;

/// <summary>
/// Data transfer object for creating or updating a candidate profile.
/// Server-controlled fields (Id, CreatedAt, IsVerified) are excluded because the client cannot set them.
/// Enforces DataAnnotations validation rules with custom error messages.
/// </summary>
public class SaveCandidateProfileRequestDto
{
    [Required(ErrorMessage = "District identifier (OcdId) is required.")]
    [StringLength(100, MinimumLength = 5, ErrorMessage = "District identifier must be between 5 and 100 characters.")]
    public string OcdId { get; set; } = string.Empty;

    [Required(ErrorMessage = "Candidate name is required.")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Candidate name must be between 2 and 100 characters.")]
    public string Name { get; set; } = string.Empty;

    [StringLength(1000, ErrorMessage = "Bio cannot exceed 1000 characters.")]
    public string? Bio { get; set; }

    [Url(ErrorMessage = "Website URL must be a valid URL.")]
    [StringLength(200, ErrorMessage = "Website URL cannot exceed 200 characters.")]
    public string? WebsiteUrl { get; set; }
}
