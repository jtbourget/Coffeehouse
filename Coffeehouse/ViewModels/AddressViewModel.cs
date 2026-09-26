using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Coffeehouse.Models;
using Coffeehouse.Services;
using Coffeehouse.Views;

namespace Coffeehouse.ViewModels
{
    /// <summary>
    /// ViewModel for managing user address input.
    /// </summary>
    public class AddressViewModel : INotifyPropertyChanged
    {
        private string _street = string.Empty;
        private string _city = string.Empty;
        private string _state = string.Empty;
        private string _zipCode = string.Empty;

        /// <summary>
        /// Gets or sets the street component of the user's address.
        /// </summary>
        public string Street
        {
            get => _street;
            set 
            { 
                if (_street != value) 
                { 
                    _street = value; 
                    OnPropertyChanged(); 
                    OnPropertyChanged(nameof(HasAddress));
                } 
            }
        }

        public bool HasAddress => !string.IsNullOrEmpty(Street);

        /// <summary>
        /// Gets or sets the city component of the user's address.
        /// </summary>
        public string City
        {
            get => _city;
            set { if (_city != value) { _city = value; OnPropertyChanged(); } }
        }

        /// <summary>
        /// Gets or sets the state component of the user's address.
        /// </summary>
        public string State
        {
            get => _state;
            set { if (_state != value) { _state = value; OnPropertyChanged(); } }
        }

        /// <summary>
        /// Gets or sets the ZIP code component of the user's address.
        /// </summary>
        public string ZipCode
        {
            get => _zipCode;
            set { if (_zipCode != value) { _zipCode = value; OnPropertyChanged(); } }
        }

        /// <summary>
        /// Gets the command used to find the ballot for the entered address.
        /// </summary>
        public ICommand FindBallotCommand { get; }

        private readonly IApiService _apiService;
        private string _searchText = string.Empty;
        
        public string SearchText
        {
            get => _searchText;
            set
            {
                if (_searchText != value)
                {
                    _searchText = value;
                    OnPropertyChanged();
                    _ = PerformSearchAsync(value);
                }
            }
        }

        public System.Collections.ObjectModel.ObservableCollection<AddressSuggestion> Suggestions { get; } = new();

        public ICommand SelectSuggestionCommand { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AddressViewModel"/> class.
        /// </summary>
        public AddressViewModel(IApiService apiService)
        {
            _apiService = apiService;
            FindBallotCommand = new Command(async () => await FindBallotAsync());
            SelectSuggestionCommand = new Command<AddressSuggestion>(async (s) => await SelectSuggestionAsync(s));
        }

        public async Task InitializeAsync()
        {
            var address = await _apiService.GetAddressAsync();
            if (address != null)
            {
                Street = address.Street;
                City = address.City;
                State = address.State;
                ZipCode = address.ZipCode;
            }
        }

        private async Task PerformSearchAsync(string query)
        {
            if (string.IsNullOrWhiteSpace(query) || query.Length < 3)
            {
                MainThread.BeginInvokeOnMainThread(() => Suggestions.Clear());
                return;
            }

            try
            {
                var results = await _apiService.GetAddressSuggestionsAsync(query);
                MainThread.BeginInvokeOnMainThread(() => 
                {
                    Suggestions.Clear();
                    if (results != null)
                    {
                        foreach (var r in results) Suggestions.Add(r);
                    }
                });
            }
            catch (Exception ex)
            {
                MainThread.BeginInvokeOnMainThread(async () => 
                {
                    if (Shell.Current != null)
                        await Shell.Current.DisplayAlert("Search Error", ex.Message, "OK");
                });
            }
        }

        private async Task SelectSuggestionAsync(AddressSuggestion? suggestion)
        {
            if (suggestion == null) return;

            var details = await _apiService.GetPlaceDetailsAsync(suggestion.PlaceId);
            if (details != null)
            {
                MainThread.BeginInvokeOnMainThread(() => 
                {
                    Street = details.Street;
                    City = details.City;
                    State = details.State;
                    ZipCode = details.ZipCode;
                    SearchText = string.Empty;
                    Suggestions.Clear();
                });
            }
        }

        private async Task FindBallotAsync()
        {
            try 
            {
                // Validate that all required address components are provided
                if (string.IsNullOrWhiteSpace(Street) || string.IsNullOrWhiteSpace(City) || 
                    string.IsNullOrWhiteSpace(State) || string.IsNullOrWhiteSpace(ZipCode))
                {
                    if (Shell.Current != null)
                        await Shell.Current.DisplayAlert("Validation", "Please fill in all address fields.", "OK");
                    return;
                }

                // Construct a new UserAddress model with the provided details
                var address = new UserAddress
                {
                    Street = Street,
                    City = City,
                    State = State,
                    ZipCode = ZipCode
                };

                bool success = await _apiService.SaveAddressAsync(address);
                if (!success) 
                {
                    if (Shell.Current != null)
                        await Shell.Current.DisplayAlert("Error", "Failed to save address.", "OK");
                    return;
                }
                
                // Navigate to ElectionsPage using Shell navigation
                if (Shell.Current != null)
                {
                    await Shell.Current.GoToAsync(nameof(ElectionsPage));
                }
            }
            catch (Exception ex)
            {
                if (Shell.Current != null)
                    await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string? name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
