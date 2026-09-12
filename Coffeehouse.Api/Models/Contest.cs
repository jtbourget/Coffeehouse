using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Coffeehouse.Api.Models
{
    /// <summary>
    /// Represents a single contest (race/office) within an election.
    /// For example: "U.S. Senator", "Governor", "Mayor", "School Board".
    /// Each contest has multiple candidates running for the office.
    /// </summary>
    public class Contest
    {
        /// <summary>
        /// Unique identifier for the contest.
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Foreign key linking this contest to its parent election.
        /// </summary>
        public int ElectionId { get; set; }

        /// <summary>
        /// The name of the office being contested (e.g., "U.S. Senator").
        /// </summary>
        [Required]
        [MaxLength(200)]
        public string OfficeName { get; set; } = string.Empty;

        /// <summary>
        /// Navigation property — the parent election this contest belongs to.
        /// JsonIgnore prevents circular reference during serialization.
        /// </summary>
        [JsonIgnore]
        [ForeignKey(nameof(ElectionId))]
        public Election? Election { get; set; }

        /// <summary>
        /// Navigation property — the candidates running in this contest.
        /// </summary>
        public List<Candidate> Candidates { get; set; } = new();
    }
}
