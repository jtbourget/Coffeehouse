namespace Coffeehouse.Models
{
    /// <summary>
    /// Represents the user's favorite candidate for a specific contest.
    /// Only one favorite per contest is allowed.
    /// </summary>
    public class FavoriteCandidate
    {
        /// <summary>
        /// Unique identifier.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The contest this favorite applies to.
        /// </summary>
        public int ContestId { get; set; }

        /// <summary>
        /// The chosen candidate's ID.
        /// </summary>
        public int CandidateId { get; set; }

        /// <summary>
        /// Navigation — the contest details.
        /// </summary>
        public Contest? Contest { get; set; }

        /// <summary>
        /// Navigation — the chosen candidate details.
        /// </summary>
        public Candidate? Candidate { get; set; }
    }
}
