using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Coffeehouse.Models;
using Coffeehouse.Services;
using Microsoft.Extensions.Logging;

namespace Coffeehouse.ViewModels
{
    /// <summary>
    /// ViewModel for the user's voting list.
    /// </summary>
    [QueryProperty(nameof(ElectionId), "ElectionId")]
    public class VotingListViewModel : INotifyPropertyChanged
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

        private readonly IApiService _apiService;
        private readonly Microsoft.Extensions.Logging.ILogger<VotingListViewModel> _logger;
        public ObservableCollection<Election> Elections { get; } = new();
        public ObservableCollection<VotingListItem> VotingList { get; } = new();

        public System.Windows.Input.ICommand SelectElectionCommand { get; }

        public VotingListViewModel(IApiService apiService, Microsoft.Extensions.Logging.ILogger<VotingListViewModel> logger)
        {
            _apiService = apiService;
            _logger = logger;
            SelectElectionCommand = new Command<Election>(async (e) => await SelectElectionAsync(e));
        }

        public async Task LoadElectionsAsync()
        {
            _logger.LogInformation("Loading elections for voting list.");
            try
            {
                var elections = await _apiService.GetElectionsAsync();
                Elections.Clear();
                foreach (var e in elections) Elections.Add(e);
                _logger.LogInformation("Loaded {Count} elections for voting list.", Elections.Count);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to load elections for voting list.");
            }
        }

        private async Task SelectElectionAsync(Election? election)
        {
            if (election == null) return;
            _logger.LogInformation("Selected election ID {Id} ('{Name}') for voting list details.", election.Id, election.Name);
            if (Shell.Current != null)
            {
                await Shell.Current.GoToAsync($"{nameof(Views.VotingListDetailsPage)}?ElectionId={election.Id}");
            }
        }

        public async Task LoadVotingListAsync()
        {
            if (ElectionId == 0) return;

            _logger.LogInformation("Loading voting list for election ID {ElectionId}.", ElectionId);
            try
            {
                CurrentElection = await _apiService.GetElectionAsync(ElectionId);
                var contests = await _apiService.GetContestsAsync(ElectionId);
                var favorites = await _apiService.GetFavoritesAsync(ElectionId);

                var favoriteDict = favorites.ToDictionary(f => f.ContestId, f => f.CandidateId);

                VotingList.Clear();
                foreach (var contest in contests)
                {
                    var item = new VotingListItem
                    {
                        OfficeName = contest.OfficeName
                    };

                    if (favoriteDict.TryGetValue(contest.Id, out int candidateId))
                    {
                        var candidate = contest.Candidates.FirstOrDefault(c => c.Id == candidateId);
                        if (candidate != null)
                        {
                            item.CandidateName = candidate.Name;
                            item.Party = candidate.Party;
                            item.PhotoUrl = candidate.PhotoUrl;
                            item.HasSelection = true;
                        }
                    }

                    VotingList.Add(item);
                }
                _logger.LogInformation("Loaded {Count} voting list items for election ID {ElectionId}.", VotingList.Count, ElectionId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to load voting list for election ID {ElectionId}.", ElectionId);
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string? name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
