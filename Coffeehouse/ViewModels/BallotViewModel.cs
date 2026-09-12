using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Coffeehouse.Models;
using Coffeehouse.Services;
using Coffeehouse.Views;

namespace Coffeehouse.ViewModels
{
    /// <summary>
    /// ViewModel for displaying an election ballot.
    /// </summary>
    public class BallotViewModel : INotifyPropertyChanged
    {
        private int _electionId;
        private Election? _currentElection;

        /// <summary>
        /// Gets or sets the ID of the current election.
        /// </summary>
        public int ElectionId
        {
            get => _electionId;
            set { if (_electionId != value) { _electionId = value; OnPropertyChanged(); } }
        }

        /// <summary>
        /// Gets or sets the current election details.
        /// </summary>
        public Election? CurrentElection
        {
            get => _currentElection;
            set { if (_currentElection != value) { _currentElection = value; OnPropertyChanged(); } }
        }

        /// <summary>
        /// Gets the collection of contests to display on the ballot.
        /// </summary>
        public ObservableCollection<ContestDisplay> Contests { get; } = new();

        /// <summary>
        /// Gets the command used to load the ballot data.
        /// </summary>
        public ICommand LoadBallotCommand { get; }

        /// <summary>
        /// Gets the command used to toggle a candidate's favorite status.
        /// </summary>
        public ICommand ToggleFavoriteCommand { get; }

        /// <summary>
        /// Gets the command used to view candidate details.
        /// </summary>
        public ICommand ViewCandidateCommand { get; }



        /// <summary>
        /// Initializes a new instance of the <see cref="BallotViewModel"/> class.
        /// </summary>
        public BallotViewModel()
        {
            LoadBallotCommand = new Command(async () => await LoadBallotAsync());
            ToggleFavoriteCommand = new Command<CandidateDisplay>(async (c) => await ToggleFavoriteAsync(c));
            ViewCandidateCommand = new Command<CandidateDisplay>(async (c) => await ViewCandidateAsync(c));

        }

        private async Task LoadBallotAsync()
        {
            // Fetch the election details, available contests, and the user's current favorites
            CurrentElection = await ApiService.Instance.GetElectionAsync(ElectionId);
            var apiContests = await ApiService.Instance.GetContestsAsync(ElectionId);
            var favorites = await ApiService.Instance.GetFavoritesAsync(ElectionId);

            // Create a lookup dictionary mapping contest IDs to favorited candidate IDs
            var favoriteDict = favorites.ToDictionary(f => f.ContestId, f => f.CandidateId);

            Contests.Clear();
            foreach (var contest in apiContests)
            {
                var contestDisplay = new ContestDisplay
                {
                    ContestId = contest.Id,
                    OfficeName = contest.OfficeName
                };

                foreach (var candidate in contest.Candidates)
                {
                    var isFavorited = favoriteDict.TryGetValue(contest.Id, out int favId) && favId == candidate.Id;
                    contestDisplay.Add(new CandidateDisplay
                    {
                        Candidate = candidate,
                        IsFavorited = isFavorited
                    });
                }
                Contests.Add(contestDisplay);
            }
        }

        private async Task ToggleFavoriteAsync(CandidateDisplay? candidateDisplay)
        {
            if (candidateDisplay == null) return;

            var contestId = candidateDisplay.Candidate.ContestId;
            var candidateId = candidateDisplay.Candidate.Id;

            if (candidateDisplay.IsFavorited)
            {
                // Remove favorite
                var success = await ApiService.Instance.RemoveFavoriteAsync(contestId);
                if (success)
                {
                    candidateDisplay.IsFavorited = false;
                }
            }
            else
            {
                // Set favorite
                var success = await ApiService.Instance.SetFavoriteAsync(contestId, candidateId);
                if (success)
                {
                    // Update UI state
                    foreach (var contest in Contests)
                    {
                        if (contest.ContestId == contestId)
                        {
                            foreach (var c in contest)
                            {
                                c.IsFavorited = (c.Candidate.Id == candidateId);
                            }
                            break;
                        }
                    }
                }
            }
        }

        private async Task ViewCandidateAsync(CandidateDisplay? candidateDisplay)
        {
            if (candidateDisplay == null) return;
            // Navigate to CandidateDetailPage using .NET 10 window-based navigation
            var navPage = Application.Current?.Windows[0].Page as NavigationPage;
            if (navPage != null)
            {
                await navPage.PushAsync(
                    new CandidateDetailPage(candidateDisplay.Candidate.Id, candidateDisplay.Candidate.ContestId));
            }
        }



        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string? name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
