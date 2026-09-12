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

        public ObservableCollection<VotingListItem> VotingList { get; } = new();

        public async Task LoadVotingListAsync()
        {
            CurrentElection = await ApiService.Instance.GetElectionAsync(ElectionId);
            var contests = await ApiService.Instance.GetContestsAsync(ElectionId);
            var favorites = await ApiService.Instance.GetFavoritesAsync(ElectionId);

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
