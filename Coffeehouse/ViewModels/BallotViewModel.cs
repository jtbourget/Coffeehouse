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

        public int ElectionId
        {
            get => _electionId;
            set { if (_electionId != value) { _electionId = value; OnPropertyChanged(); } }
        }

        public Election? CurrentElection
        {
            get => _currentElection;
            set { if (_currentElection != value) { _currentElection = value; OnPropertyChanged(); } }
        }

        public ObservableCollection<ContestDisplay> Contests { get; } = new();

        public ICommand LoadBallotCommand { get; }
        public ICommand ToggleFavoriteCommand { get; }
        public ICommand ViewCandidateCommand { get; }
        public ICommand ViewVotingListCommand { get; }

        public BallotViewModel()
        {
            LoadBallotCommand = new Command(async () => await LoadBallotAsync());
            ToggleFavoriteCommand = new Command<CandidateDisplay>(async (c) => await ToggleFavoriteAsync(c));
            ViewCandidateCommand = new Command<CandidateDisplay>(async (c) => await ViewCandidateAsync(c));
            ViewVotingListCommand = new Command(async () => await ViewVotingListAsync());
        }

        private async Task LoadBallotAsync()
        {
            CurrentElection = await ApiService.Instance.GetElectionAsync(ElectionId);
            var apiContests = await ApiService.Instance.GetContestsAsync(ElectionId);
            var favorites = await ApiService.Instance.GetFavoritesAsync(ElectionId);

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
                    contestDisplay.Candidates.Add(new CandidateDisplay
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
                            foreach (var c in contest.Candidates)
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

        private async Task ViewVotingListAsync()
        {
            // Navigate to VotingListPage using .NET 10 window-based navigation
            var navPage = Application.Current?.Windows[0].Page as NavigationPage;
            if (navPage != null)
            {
                await navPage.PushAsync(new VotingListPage(ElectionId));
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string? name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
