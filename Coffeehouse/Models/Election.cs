namespace Coffeehouse.Models
{
    /// <summary>
    /// Represents an election/ballot event received from the API.
    /// Contains the election date, voting location, and associated contests.
    /// </summary>
    public class Election
    {
        /// <summary>
        /// Unique identifier for the election.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Display name of the election (e.g., "2026 General Election").
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// The date when voting takes place.
        /// </summary>
        public DateTime ElectionDate { get; set; }

        /// <summary>
        /// Name of the voting location (e.g., "Lincoln Community Center").
        /// </summary>
        public string VotingLocationName { get; set; } = string.Empty;

        /// <summary>
        /// Full street address of the voting location.
        /// </summary>
        public string VotingLocationAddress { get; set; } = string.Empty;

        /// <summary>
        /// The contests (races/offices) on this ballot.
        /// </summary>
        public List<Contest> Contests { get; set; } = new();
    }
}
