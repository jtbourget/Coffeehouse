using System.Windows.Input;
using Coffeehouse.Models;
using Coffeehouse.Services;

namespace Coffeehouse.ViewModels;

[QueryProperty(nameof(ProfileId), "id")]
public class CandidateProfileDetailViewModel : ViewModelBase
{
    private readonly ICandidateProfileApiClient _apiClient;
    private int _profileId;
    private CandidateProfileDisplay? _profile;

    public int ProfileId
    {
        get => _profileId;
        set
        {
            _profileId = value;
            OnPropertyChanged();
            _ = LoadProfileAsync(value);
        }
    }

    public CandidateProfileDisplay? Profile
    {
        get => _profile;
        set { if (_profile != value) { _profile = value; OnPropertyChanged(); } }
    }

    public ICommand EditCommand { get; }
    public ICommand DeleteCommand { get; }
    public ICommand OpenWebsiteCommand { get; }

    public CandidateProfileDetailViewModel(ICandidateProfileApiClient apiClient)
    {
        _apiClient = apiClient;

        EditCommand = new Command(async () => await NavigateToEditAsync());
        DeleteCommand = new Command(async () => await DeleteAsync());
        OpenWebsiteCommand = new Command(async () => await OpenWebsiteAsync());
    }

    public async Task LoadProfileAsync(int id)
    {
        if (id <= 0) return;

        IsBusy = true;
        ErrorMessage = null;

        try
        {
            var result = await _apiClient.GetProfileAsync(id);
            if (result.IsSuccess && result.Value != null)
            {
                Profile = result.Value;
            }
            else if (result.IsNotFound)
            {
                ErrorMessage = "That candidate profile no longer exists.";
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

    private async Task NavigateToEditAsync()
    {
        if (Profile == null || Shell.Current == null) return;
        await Shell.Current.GoToAsync($"CandidateProfileEditPage?id={Profile.Id}");
    }

    private async Task DeleteAsync()
    {
        if (Profile == null) return;

        var confirmed = await Application.Current!.Windows[0].Page!.DisplayAlertAsync(
            "Delete Profile",
            $"Are you sure you want to delete profile for '{Profile.Name}'?",
            "Delete",
            "Cancel");

        if (!confirmed) return;

        IsBusy = true;
        ErrorMessage = null;

        try
        {
            var result = await _apiClient.DeleteProfileAsync(Profile.Id);
            if (result.IsSuccess || result.IsNotFound)
            {
                if (Shell.Current != null)
                {
                    await Shell.Current.GoToAsync("..");
                }
            }
            else if (result.IsUnauthorized)
            {
                await SendToLoginAsync("Please log in to make changes.");
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

    private async Task OpenWebsiteAsync()
    {
        if (string.IsNullOrWhiteSpace(Profile?.WebsiteUrl)) return;

        try
        {
            var uri = new Uri(Profile.WebsiteUrl);
            await Browser.Default.OpenAsync(uri, BrowserLaunchMode.SystemPreferred);
        }
        catch
        {
            ErrorMessage = "Could not open website URL.";
        }
    }
}
