using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Coffeehouse.Api.Models
{
    /// <summary>
    /// Represents a candidate running for office in a specific contest.
    /// Stores their personal info, photo, campaign website, and biography.
    /// </summary>
    public class Candidate
    {
        /// <summary>
        /// Unique identifier for the candidate.
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Foreign key linking this candidate to the contest they are running in.
        /// </summary>
        public int ContestId { get; set; }

        /// <summary>
        /// Full name of the candidate.
        /// </summary>
        [Required]
        [MaxLength(200)]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Political party affiliation (e.g., "Democratic", "Republican", "Independent").
        /// </summary>
        [MaxLength(100)]
        public string Party { get; set; } = string.Empty;

        /// <summary>
        /// URL to the candidate's official photo.
        /// </summary>
        [MaxLength(500)]
        public string PhotoUrl { get; set; } = string.Empty;

        /// <summary>
        /// URL to the candidate's campaign website.
        /// </summary>
        [MaxLength(500)]
        public string CampaignWebsite { get; set; } = string.Empty;

        /// <summary>
        /// Biographical information and platform summary for the candidate.
        /// </summary>
        public string Bio { get; set; } = string.Empty;

        /// <summary>
        /// Navigation property — the contest this candidate is running in.
        /// JsonIgnore prevents circular reference during serialization.
        /// </summary>
        [JsonIgnore]
        [ForeignKey(nameof(ContestId))]
        public Contest? Contest { get; set; }
    }
}
