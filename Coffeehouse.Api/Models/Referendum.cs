using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Coffeehouse.Api.Models
{
    /// <summary>
    /// Represents a ballot measure or referendum in an election.
    /// </summary>
    public class Referendum
    {
        [Key]
        public int Id { get; set; }

        public int ElectionId { get; set; }

        [Required]
        [MaxLength(200)]
        public string Title { get; set; } = string.Empty;

        [MaxLength(2000)]
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// The Open Civic Data Identifier (OCD-ID) representing the district/geography of this referendum.
        /// </summary>
        [MaxLength(200)]
        public string OcdId { get; set; } = string.Empty;

        [JsonIgnore]
        [ForeignKey(nameof(ElectionId))]
        public Election? Election { get; set; }
    }
}
