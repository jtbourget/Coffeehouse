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

        public string Street
        {
            get => _street;
            set { if (_street != value) { _street = value; OnPropertyChanged(); } }
        }

        public string City
        {
            get => _city;
            set { if (_city != value) { _city = value; OnPropertyChanged(); } }
        }

        public string State
        {
            get => _state;
            set { if (_state != value) { _state = value; OnPropertyChanged(); } }
        }

        public string ZipCode
        {
            get => _zipCode;
            set { if (_zipCode != value) { _zipCode = value; OnPropertyChanged(); } }
        }

        public ICommand FindBallotCommand { get; }

        public AddressViewModel()
        {
            FindBallotCommand = new Command(async () => await FindBallotAsync());
            _ = LoadAddressAsync();
        }

        private async Task LoadAddressAsync()
        {
            var address = await ApiService.Instance.GetAddressAsync();
            if (address != null)
            {
                Street = address.Street;
                City = address.City;
                State = address.State;
                ZipCode = address.ZipCode;
            }
        }

        private async Task FindBallotAsync()
        {
            try 
            {
                if (string.IsNullOrWhiteSpace(Street) || string.IsNullOrWhiteSpace(City) || 
                    string.IsNullOrWhiteSpace(State) || string.IsNullOrWhiteSpace(ZipCode))
                {
                    if (Shell.Current != null)
                        await Shell.Current.DisplayAlert("Validation", "Please fill in all address fields.", "OK");
                    return;
                }

                var address = new UserAddress
                {
                    Street = Street,
                    City = City,
                    State = State,
                    ZipCode = ZipCode
                };

                bool success = await ApiService.Instance.SaveAddressAsync(address);
                if (!success) 
                {
                    // Do nothing, ApiService shows the detailed error
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
