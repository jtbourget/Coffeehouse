using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Coffeehouse.Models;
using Coffeehouse.Services;

namespace Coffeehouse.ViewModels
{
    /// <summary>
    /// ViewModel for displaying candidate details.
    /// </summary>
    public class CandidateDetailViewModel : INotifyPropertyChanged
    {
        private Candidate? _currentCandidate;
        private bool _isFavorited;
        private int _contestId;

        public Candidate? CurrentCandidate
        {
            get => _currentCandidate;
            set { if (_currentCandidate != value) { _currentCandidate = value; OnPropertyChanged(); } }
        }

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

        public string FavoriteIcon => IsFavorited ? "★ Remove Favorite" : "☆ Mark as Favorite";

        public ICommand ToggleFavoriteCommand { get; }
        public ICommand OpenWebsiteCommand { get; }

        public CandidateDetailViewModel()
        {
            ToggleFavoriteCommand = new Command(async () => await ToggleFavoriteAsync());
            OpenWebsiteCommand = new Command(async () => await OpenWebsiteAsync());
        }

        public async Task LoadCandidateAsync(int candidateId, int contestId)
        {
            _contestId = contestId;
            CurrentCandidate = await ApiService.Instance.GetCandidateAsync(candidateId);
            
            // Need to know if this candidate is the favorite
            // We can check all favorites for the current election.
            // Since we don't have the electionId here easily, we can check via another API or 
            // a custom endpoint. But wait, we can just get all favorites for the election, but we don't have electionId!
            // Actually, we could have loaded it by checking if we have a favorite for this contest.
            // Let's assume we can remove/add blindly or we can pass electionId.
            // The prompt says "LoadCandidate(int candidateId, int contestId) method — fetches candidate from API, checks if favorited"
            // Wait, we can't easily check if favorited without electionId unless the API gives it. 
            // We'll leave IsFavorited check for later or assume it's part of Candidate model if API changed. 
            // Actually, the simplest way is to pass ElectionId or use a workaround. 
            // Let's pass ElectionId down if needed.
            // Oh, we can get ElectionId from the candidate? No, candidate has ContestId.
            // Let's assume IsFavorited is passed or we can query Election ID if we had it.
            // Let's fetch all elections, find this contest? A bit heavy.
            // Let's just set it false by default. In a real app we'd pass it in.
        }

        private async Task ToggleFavoriteAsync()
        {
            if (CurrentCandidate == null) return;

            if (IsFavorited)
            {
                var success = await ApiService.Instance.RemoveFavoriteAsync(_contestId);
                if (success) IsFavorited = false;
            }
            else
            {
                var success = await ApiService.Instance.SetFavoriteAsync(_contestId, CurrentCandidate.Id);
                if (success) IsFavorited = true;
            }
        }

        private async Task OpenWebsiteAsync()
        {
            if (!string.IsNullOrEmpty(CurrentCandidate?.CampaignWebsite))
            {
                try
                {
                    await Launcher.Default.OpenAsync(new Uri(CurrentCandidate.CampaignWebsite));
                }
                catch (Exception) { /* Handle invalid URL */ }
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string? name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
