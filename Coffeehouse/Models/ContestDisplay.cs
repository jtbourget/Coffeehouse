using System.Collections.ObjectModel;

namespace Coffeehouse.Models
{
    /// <summary>
    /// Wraps a Contest with its candidates as CandidateDisplay items for the ballot view.
    /// Used for grouped display in the BallotPage CollectionView.
    /// </summary>
    public class ContestDisplay
    {
        /// <summary>
        /// The office name for this contest.
        /// </summary>
        public string OfficeName { get; set; } = string.Empty;

        /// <summary>
        /// The contest ID.
        /// </summary>
        public int ContestId { get; set; }

        /// <summary>
        /// The candidates running, with favorite state.
        /// </summary>
        public ObservableCollection<CandidateDisplay> Candidates { get; set; } = new();
    }
}
