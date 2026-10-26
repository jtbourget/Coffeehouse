using System.Collections.ObjectModel;
using System.Windows.Input;
using Coffeehouse.Models;
using Coffeehouse.Services;

namespace Coffeehouse.ViewModels;

public class CandidateProfilesListViewModel : ViewModelBase
{
    private readonly ICandidateProfileApiClient _apiClient;
    private readonly IAuthService _authService;
    private string _searchText = string.Empty;
    private bool _isLoggedIn;

    public ObservableCollection<CandidateProfileDisplay> Profiles { get; } = new();

    public string SearchText
    {
        get => _searchText;
        set
        {
            if (_searchText != value)
            {
                _searchText = value;
                OnPropertyChanged();
                _ = LoadProfilesAsync();
            }
        }
    }

    public bool IsLoggedIn
    {
        get => _isLoggedIn;
        set
        {
            if (_isLoggedIn != value)
            {
                _isLoggedIn = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(AuthButtonText));
            }
        }
    }

    public string AuthButtonText => IsLoggedIn ? "Log out" : "Log in";

    public ICommand LoadProfilesCommand { get; }
    public ICommand AuthActionCommand { get; }
    public ICommand AddProfileCommand { get; }
    public ICommand SelectProfileCommand { get; }

    public CandidateProfilesListViewModel(ICandidateProfileApiClient apiClient, IAuthService authService)
    {
        _apiClient = apiClient;
        _authService = authService;

        LoadProfilesCommand = new Command(async () => await LoadProfilesAsync());
        AuthActionCommand = new Command(async () => await HandleAuthActionAsync());
        AddProfileCommand = new Command(async () => await NavigateToAddProfileAsync());
        SelectProfileCommand = new Command<CandidateProfileDisplay>(async (p) => await NavigateToDetailAsync(p));
    }

    public async Task RefreshStateAsync()
    {
        IsLoggedIn = await _authService.IsLoggedInAsync();
        await LoadProfilesAsync();
    }

    public async Task LoadProfilesAsync()
    {
        if (IsBusy) return;

        IsBusy = true;
        ErrorMessage = null;

        try
        {
            var result = await _apiClient.GetProfilesAsync(search: SearchText);
            if (result.IsSuccess && result.Value != null)
            {
                Profiles.Clear();
                foreach (var profile in result.Value)
                {
                    Profiles.Add(profile);
                }
            }
            else
            {
                ErrorMessage = result.ErrorMessage;
            }
        }
        finally
        {
            IsBusy = false;
        }
    }

    private async Task HandleAuthActionAsync()
    {
        if (IsLoggedIn)
        {
            await _authService.LogoutAsync();
            IsLoggedIn = false;
        }
        else
        {
            if (Shell.Current != null)
            {
                await Shell.Current.GoToAsync("LoginPage");
            }
        }
    }

    private async Task NavigateToAddProfileAsync()
    {
        if (Shell.Current != null)
        {
            await Shell.Current.GoToAsync("CandidateProfileEditPage");
        }
    }

    private async Task NavigateToDetailAsync(CandidateProfileDisplay? profile)
    {
        if (profile == null) return;
        if (Shell.Current != null)
        {
            await Shell.Current.GoToAsync($"CandidateProfileDetailPage?id={profile.Id}");
        }
    }
}
