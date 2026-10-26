using System.ComponentModel.DataAnnotations;

namespace Coffeehouse.Api.Configuration;

public class AuthenticationOptions
{
    public const string SectionName = "Authentication";

    [Required]
    public bool Enabled { get; set; } = true;

    [Required]
    public string Issuer { get; set; } = "CoffeehouseTeachingApi";

    [Required]
    public string Audience { get; set; } = "CoffeehouseStudents";

    [Range(1, 1440)]
    public int AccessTokenMinutes { get; set; } = 60;

    [Required]
    public string SigningKey { get; set; } = string.Empty;

    public bool AutoGenerateSigningKey { get; set; } = true;
}
