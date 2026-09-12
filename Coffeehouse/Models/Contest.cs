namespace Coffeehouse.Models
{
    /// <summary>
    /// Represents a contest (race/office) within an election.
    /// For example: "Governor", "U.S. Senator", "Mayor".
    /// </summary>
    public class Contest
    {
        /// <summary>
        /// Unique identifier for the contest.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Foreign key to the parent election.
        /// </summary>
        public int ElectionId { get; set; }

        /// <summary>
        /// Name of the office (e.g., "U.S. Senator").
        /// </summary>
        public string OfficeName { get; set; } = string.Empty;

        /// <summary>
        /// The candidates running in this contest.
        /// </summary>
        public List<Candidate> Candidates { get; set; } = new();
    }
}
