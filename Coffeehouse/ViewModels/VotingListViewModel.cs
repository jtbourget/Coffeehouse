using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Coffeehouse.Models;
using Coffeehouse.Services;

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
        public ObservableCollection<Election> Elections { get; } = new();
        public ObservableCollection<VotingListItem> VotingList { get; } = new();

        public System.Windows.Input.ICommand SelectElectionCommand { get; }

        public VotingListViewModel(IApiService apiService)
        {
            _apiService = apiService;
            SelectElectionCommand = new Command<Election>(async (e) => await SelectElectionAsync(e));
        }

        public async Task LoadElectionsAsync()
        {
            var elections = await _apiService.GetElectionsAsync();
            Elections.Clear();
            foreach (var e in elections) Elections.Add(e);
        }

        private async Task SelectElectionAsync(Election? election)
        {
            if (election == null) return;
            if (Shell.Current != null)
            {
                await Shell.Current.GoToAsync($"{nameof(Views.VotingListDetailsPage)}?ElectionId={election.Id}");
            }
        }

        public async Task LoadVotingListAsync()
        {
            if (ElectionId == 0) return;

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
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string? name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
