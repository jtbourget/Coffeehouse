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
    /// ViewModel for listing available elections.
    /// </summary>
    public class ElectionsViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// Gets the collection of available elections.
        /// </summary>
        public ObservableCollection<Election> Elections { get; } = new();

        /// <summary>
        /// Gets the command used to load the elections list.
        /// </summary>
        public ICommand LoadElectionsCommand { get; }

        /// <summary>
        /// Gets the command used to select an election and view its ballot.
        /// </summary>
        public ICommand SelectElectionCommand { get; }

        private readonly IApiService _apiService;
        private readonly Microsoft.Extensions.Logging.ILogger<ElectionsViewModel> _logger;
        private bool _isBusy;
        public bool IsBusy
        {
            get => _isBusy;
            set
            {
                if (_isBusy != value)
                {
                    _isBusy = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ElectionsViewModel"/> class.
        /// </summary>
        public ElectionsViewModel(IApiService apiService, Microsoft.Extensions.Logging.ILogger<ElectionsViewModel> logger)
        {
            _apiService = apiService;
            _logger = logger;
            LoadElectionsCommand = new Command(async () => await InitializeAsync());
            SelectElectionCommand = new Command<Election>(async (election) => await SelectElectionAsync(election));
        }

        public async Task InitializeAsync()
        {
            IsBusy = true;
            _logger.LogInformation("Loading elections list.");
            try
            {
                var elections = await _apiService.GetElectionsAsync();
                Elections.Clear();
                foreach (var election in elections)
                {
                    Elections.Add(election);
                }
                _logger.LogInformation("Loaded {Count} elections.", Elections.Count);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading elections list.");
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task SelectElectionAsync(Election? election)
        {
            if (election == null) return;

            _logger.LogInformation("Selected election ID {Id} ('{Name}'). Navigating to BallotPage.", election.Id, election.Name);
            // Navigate to BallotPage using Shell navigation
            if (Shell.Current != null)
            {
                await Shell.Current.GoToAsync($"{nameof(BallotPage)}?id={election.Id}");
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string? name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
