using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Coffeehouse.Models;
using Coffeehouse.Services;
using Microsoft.Extensions.Logging;

namespace Coffeehouse.ViewModels
{
    /// <summary>
    /// ViewModel for displaying candidate details.
    /// </summary>
    [QueryProperty(nameof(CandidateId), "candidateId")]
    [QueryProperty(nameof(ContestId), "contestId")]
    public class CandidateDetailViewModel : INotifyPropertyChanged
    {
        private readonly IApiService _apiService;
        private readonly Microsoft.Extensions.Logging.ILogger<CandidateDetailViewModel> _logger;
        private Candidate? _currentCandidate;
        private bool _isFavorited;
        private int _contestId;
        private int _candidateId;

        public int CandidateId
        {
            get => _candidateId;
            set { _candidateId = value; }
        }

        public int ContestId
        {
            get => _contestId;
            set { _contestId = value; }
        }

        /// <summary>
        /// Gets or sets the current candidate being detailed.
        /// </summary>
        public Candidate? CurrentCandidate
        {
            get => _currentCandidate;
            set { if (_currentCandidate != value) { _currentCandidate = value; OnPropertyChanged(); } }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the current candidate is favorited.
        /// </summary>
        public bool IsFavorited
        {
            get => _isFavorited;
            set
            {
                if (_isFavorited != value)
                {
                    _isFavorited = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(FavoriteIcon));
                }
            }
        }

        /// <summary>
        /// Gets the text to display for the favorite icon, depending on favorited state.
        /// </summary>
        public string FavoriteIcon => IsFavorited ? "★ Remove Favorite" : "☆ Mark as Favorite";

        /// <summary>
        /// Gets the command used to toggle the favorite state for the candidate.
        /// </summary>
        public ICommand ToggleFavoriteCommand { get; }

        /// <summary>
        /// Gets the command used to open the candidate's campaign website.
        /// </summary>
        public ICommand OpenWebsiteCommand { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CandidateDetailViewModel"/> class.
        /// </summary>
        public CandidateDetailViewModel(IApiService apiService, Microsoft.Extensions.Logging.ILogger<CandidateDetailViewModel> logger)
        {
            _apiService = apiService;
            _logger = logger;
            ToggleFavoriteCommand = new Command(async () => await ToggleFavoriteAsync());
            OpenWebsiteCommand = new Command(async () => await OpenWebsiteAsync());
        }

        /// <summary>
        /// Loads the details for the specified candidate and contest.
        /// </summary>
        /// <param name="candidateId">The candidate's unique identifier.</param>
        /// <param name="contestId">The contest's unique identifier.</param>
        public async Task LoadCandidateAsync(int candidateId, int contestId)
        {
            _contestId = contestId;
            _logger.LogInformation("Loading candidate details for candidate ID {CandidateId}, contest ID {ContestId}.", candidateId, contestId);
            try
            {
                CurrentCandidate = await _apiService.GetCandidateAsync(candidateId);
                _logger.LogInformation("Successfully loaded details for candidate '{CandidateName}' (ID {CandidateId}).", CurrentCandidate?.Name, candidateId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to load candidate details for ID {CandidateId}.", candidateId);
            }
        }

        private async Task ToggleFavoriteAsync()
        {
            if (CurrentCandidate == null) return;

            _logger.LogInformation("Toggling favorite for candidate ID {CandidateId} (contest ID {ContestId}). Currently favorited: {IsFavorited}", CurrentCandidate.Id, _contestId, IsFavorited);
            try
            {
                if (IsFavorited)
                {
                    var success = await _apiService.RemoveFavoriteAsync(_contestId);
                    if (success)
                    {
                        IsFavorited = false;
                        _logger.LogInformation("Removed candidate {CandidateId} from favorites.", CurrentCandidate.Id);
                    }
                }
                else
                {
                    var success = await _apiService.SetFavoriteAsync(_contestId, CurrentCandidate.Id);
                    if (success)
                    {
                        IsFavorited = true;
                        _logger.LogInformation("Added candidate {CandidateId} to favorites.", CurrentCandidate.Id);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to toggle favorite for candidate ID {CandidateId}.", CurrentCandidate.Id);
            }
        }

        private async Task OpenWebsiteAsync()
        {
            if (!string.IsNullOrEmpty(CurrentCandidate?.CampaignWebsite))
            {
                _logger.LogInformation("Opening campaign website: {Website}", CurrentCandidate.CampaignWebsite);
                try
                {
                    await Launcher.Default.OpenAsync(new Uri(CurrentCandidate.CampaignWebsite));
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "Failed to open campaign website URL: {Website}", CurrentCandidate.CampaignWebsite);
                }
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string? name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
