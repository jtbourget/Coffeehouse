using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Coffeehouse.Models;
using Coffeehouse.Services;
using Coffeehouse.Views;
using Microsoft.Extensions.Logging;

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



        private readonly IApiService _apiService;
        private readonly Microsoft.Extensions.Logging.ILogger<BallotViewModel> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="BallotViewModel"/> class.
        /// </summary>
        public BallotViewModel(IApiService apiService, Microsoft.Extensions.Logging.ILogger<BallotViewModel> logger)
        {
            _apiService = apiService;
            _logger = logger;
            LoadBallotCommand = new Command(async () => await LoadBallotAsync());
            ToggleFavoriteCommand = new Command<CandidateDisplay>(async (c) => await ToggleFavoriteAsync(c));
            ViewCandidateCommand = new Command<CandidateDisplay>(async (c) => await ViewCandidateAsync(c));
        }

        public async Task LoadBallotAsync()
        {
            _logger.LogInformation("Loading ballot for election ID {ElectionId}.", ElectionId);
            try
            {
                // Fetch the election details, available contests, and the user's current favorites
                CurrentElection = await _apiService.GetElectionAsync(ElectionId);
                var apiContests = await _apiService.GetContestsAsync(ElectionId);
                var favorites = await _apiService.GetFavoritesAsync(ElectionId);

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
                _logger.LogInformation("Loaded {Count} contests for election ID {ElectionId}.", Contests.Count, ElectionId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading ballot for election ID {ElectionId}.", ElectionId);
            }
        }

        private async Task ToggleFavoriteAsync(CandidateDisplay? candidateDisplay)
        {
            if (candidateDisplay == null) return;

            var contestId = candidateDisplay.Candidate.ContestId;
            var candidateId = candidateDisplay.Candidate.Id;
            _logger.LogInformation("Toggling favorite for candidate ID {CandidateId} (currently favorited: {IsFavorited}) in contest {ContestId}.", candidateId, candidateDisplay.IsFavorited, contestId);

            try
            {
                if (candidateDisplay.IsFavorited)
                {
                    // Remove favorite
                    var success = await _apiService.RemoveFavoriteAsync(contestId);
                    if (success)
                    {
                        candidateDisplay.IsFavorited = false;
                        _logger.LogInformation("Favorite removed for candidate {CandidateId}.", candidateId);
                    }
                }
                else
                {
                    // Set favorite
                    var success = await _apiService.SetFavoriteAsync(contestId, candidateId);
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
                        _logger.LogInformation("Candidate {CandidateId} set as favorite for contest {ContestId}.", candidateId, contestId);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error toggling favorite for candidate {CandidateId}.", candidateId);
            }
        }

        private async Task ViewCandidateAsync(CandidateDisplay? candidateDisplay)
        {
            if (candidateDisplay == null) return;
            
            _logger.LogInformation("Navigating to candidate details for ID {CandidateId}.", candidateDisplay.Candidate.Id);
            if (Shell.Current != null)
            {
                await Shell.Current.GoToAsync($"{nameof(Views.CandidateDetailPage)}?candidateId={candidateDisplay.Candidate.Id}&contestId={candidateDisplay.Candidate.ContestId}");
            }
        }



        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string? name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
