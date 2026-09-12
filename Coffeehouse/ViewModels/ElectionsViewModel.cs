using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Coffeehouse.Models;
using Coffeehouse.Services;
using Coffeehouse.Views;

namespace Coffeehouse.ViewModels
{
    /// <summary>
    /// ViewModel for listing available elections.
    /// </summary>
    public class ElectionsViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Election> Elections { get; } = new();

        public ICommand LoadElectionsCommand { get; }
        public ICommand SelectElectionCommand { get; }

        public ElectionsViewModel()
        {
            LoadElectionsCommand = new Command(async () => await LoadElectionsAsync());
            SelectElectionCommand = new Command<Election>(async (election) => await SelectElectionAsync(election));

            _ = LoadElectionsAsync();
        }

        private async Task LoadElectionsAsync()
        {
            var elections = await ApiService.Instance.GetElectionsAsync();
            Elections.Clear();
            foreach (var election in elections)
            {
                Elections.Add(election);
            }
        }

        private async Task SelectElectionAsync(Election? election)
        {
            if (election == null) return;

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
