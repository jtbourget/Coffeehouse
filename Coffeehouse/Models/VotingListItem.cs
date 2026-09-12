namespace Coffeehouse.Models
{
    /// <summary>
    /// Represents one row in the user's voting list — an office and their chosen candidate.
    /// </summary>
    public class VotingListItem
    {
        /// <summary>
        /// The office name (e.g., "Governor").
        /// </summary>
        public string OfficeName { get; set; } = string.Empty;

        /// <summary>
        /// The chosen candidate's name, or "No selection" if none.
        /// </summary>
        public string CandidateName { get; set; } = "No selection";

        /// <summary>
        /// The chosen candidate's party.
        /// </summary>
        public string Party { get; set; } = string.Empty;

        /// <summary>
        /// The chosen candidate's photo URL.
        /// </summary>
        public string PhotoUrl { get; set; } = string.Empty;

        /// <summary>
        /// Whether a candidate has been selected for this office.
        /// </summary>
        public bool HasSelection { get; set; }
    }
}
