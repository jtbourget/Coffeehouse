using System.ComponentModel.DataAnnotations;

namespace Coffeehouse.Api.Configuration;

public class DataSourceOptions
{
    public const string SectionName = "DataSource";

    [Required]
    [AllowedValues("Json", "SqlServer")]
    public string Provider { get; set; } = "Json";

    [Required]
    public string JsonFilePath { get; set; } = "Data/CandidateProfiles.json";

    public bool AutoStartDocker { get; set; } = true;

    public bool MigrateOnStartup { get; set; } = true;
}
