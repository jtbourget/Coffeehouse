using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Coffeehouse.Models
{
    /// <summary>
    /// Wraps a Candidate with display state for the ballot view.
    /// Adds IsFavorited property for UI binding with change notification.
    /// </summary>
    public class CandidateDisplay : INotifyPropertyChanged
    {
        private bool _isFavorited;

        /// <summary>
        /// The underlying candidate data.
        /// </summary>
        public Candidate Candidate { get; set; } = new();

        /// <summary>
<<<<<<< HEAD
        /// Pass-through property for PhotoUrl to avoid nested binding issues.
=======
        /// Pass-through property for PhotoUrl to avoid nested binding issues in MAUI CollectionView.
>>>>>>> 1c6e4c5e0c151cb1c1faf410a3bc0daa180b6ca2
        /// </summary>
        public string PhotoUrl => Candidate?.PhotoUrl ?? string.Empty;

        /// <summary>
        /// Whether this candidate is the user's favorite for their contest.
        /// Supports property change notification for real-time UI updates.
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
                }
            }
        }

        /// <summary>
        /// Display text for the favorite button.
        /// </summary>
        public string FavoriteIcon => IsFavorited ? "★" : "☆";

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string? name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
            // Also notify FavoriteIcon changed when IsFavorited changes
            if (name == nameof(IsFavorited))
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(FavoriteIcon)));
            }
        }
    }
}
