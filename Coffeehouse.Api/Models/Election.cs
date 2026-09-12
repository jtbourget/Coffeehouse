using System.ComponentModel.DataAnnotations;

namespace Coffeehouse.Api.Models
{
    /// <summary>
    /// Represents an election/ballot event with its date and voting location information.
    /// Each election contains multiple contests (races) that voters can participate in.
    /// </summary>
    public class Election
    {
        /// <summary>
        /// Unique identifier for the election.
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Display name of the election (e.g., "2026 General Election").
        /// </summary>
        [Required]
        [MaxLength(200)]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// The date when voting takes place.
        /// </summary>
        public DateTime ElectionDate { get; set; }

        /// <summary>
        /// Name of the voting location (e.g., "Lincoln Community Center").
        /// </summary>
        [MaxLength(200)]
        public string VotingLocationName { get; set; } = string.Empty;

        /// <summary>
        /// Full street address of the voting location.
        /// </summary>
        [MaxLength(500)]
        public string VotingLocationAddress { get; set; } = string.Empty;

        /// <summary>
        /// Navigation property — the contests/races on this ballot.
        /// </summary>
        public List<Contest> Contests { get; set; } = new();
    }
}
