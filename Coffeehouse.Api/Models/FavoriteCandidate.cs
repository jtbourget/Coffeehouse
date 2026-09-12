using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Coffeehouse.Api.Models
{
    /// <summary>
    /// Represents the user's favorite (chosen) candidate for a specific contest.
    /// Only one favorite is allowed per contest, enforced by a unique index on ContestId.
    /// This allows the user to build a complete voting list across all races.
    /// </summary>
    [Index(nameof(ContestId), IsUnique = true)]
    public class FavoriteCandidate
    {
        /// <summary>
        /// Unique identifier for this favorite record.
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// The contest this favorite applies to. Unique — only one favorite per contest.
        /// </summary>
        public int ContestId { get; set; }

        /// <summary>
        /// The candidate the user has chosen as their favorite for this contest.
        /// </summary>
        public int CandidateId { get; set; }

        /// <summary>
        /// Navigation property — the contest this favorite belongs to.
        /// </summary>
        [ForeignKey(nameof(ContestId))]
        public Contest? Contest { get; set; }

        /// <summary>
        /// Navigation property — the chosen candidate.
        /// </summary>
        [ForeignKey(nameof(CandidateId))]
        public Candidate? Candidate { get; set; }
    }
}
