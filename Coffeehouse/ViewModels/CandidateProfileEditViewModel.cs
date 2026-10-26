using System.Windows.Input;
using Coffeehouse.Models;
using Coffeehouse.Services;

namespace Coffeehouse.ViewModels;

[QueryProperty(nameof(ProfileId), "id")]
public class CandidateProfileEditViewModel : ViewModelBase
{
    private readonly ICandidateProfileApiClient _apiClient;
    private int _profileId;
    private string _name = string.Empty;
    private string _ocdId = string.Empty;
    private string? _bio;
    private string? _websiteUrl;

    private string? _nameError;
    private string? _ocdIdError;
    private string? _bioError;
    private string? _websiteUrlError;
    private string? _generalError;

    public int ProfileId
    {
        get => _profileId;
        set
        {
            _profileId = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(PageTitle));
            OnPropertyChanged(nameof(IsEditMode));
            if (value > 0)
            {
                _ = LoadExistingProfileAsync(value);
            }
        }
    }

    public bool IsEditMode => ProfileId > 0;
    public string PageTitle => IsEditMode ? "Edit Candidate Profile" : "New Candidate Profile";

    public string Name
    {
        get => _name;
        set { if (_name != value) { _name = value; OnPropertyChanged(); NameError = null; } }
    }

    public string OcdId
    {
        get => _ocdId;
        set { if (_ocdId != value) { _ocdId = value; OnPropertyChanged(); OcdIdError = null; } }
    }

    public string? Bio
    {
        get => _bio;
        set { if (_bio != value) { _bio = value; OnPropertyChanged(); BioError = null; } }
    }

    public string? WebsiteUrl
    {
        get => _websiteUrl;
        set { if (_websiteUrl != value) { _websiteUrl = value; OnPropertyChanged(); WebsiteUrlError = null; } }
    }

    public string? NameError
    {
        get => _nameError;
        set { if (_nameError != value) { _nameError = value; OnPropertyChanged(); OnPropertyChanged(nameof(HasNameError)); } }
    }
    public bool HasNameError => !string.IsNullOrEmpty(_nameError);

    public string? OcdIdError
    {
        get => _ocdIdError;
        set { if (_ocdIdError != value) { _ocdIdError = value; OnPropertyChanged(); OnPropertyChanged(nameof(HasOcdIdError)); } }
    }
    public bool HasOcdIdError => !string.IsNullOrEmpty(_ocdIdError);

    public string? BioError
    {
        get => _bioError;
        set { if (_bioError != value) { _bioError = value; OnPropertyChanged(); OnPropertyChanged(nameof(HasBioError)); } }
    }
    public bool HasBioError => !string.IsNullOrEmpty(_bioError);

    public string? WebsiteUrlError
    {
        get => _websiteUrlError;
        set { if (_websiteUrlError != value) { _websiteUrlError = value; OnPropertyChanged(); OnPropertyChanged(nameof(HasWebsiteUrlError)); } }
    }
    public bool HasWebsiteUrlError => !string.IsNullOrEmpty(_websiteUrlError);

    public string? GeneralError
    {
        get => _generalError;
        set { if (_generalError != value) { _generalError = value; OnPropertyChanged(); OnPropertyChanged(nameof(HasGeneralError)); } }
    }
    public bool HasGeneralError => !string.IsNullOrEmpty(_generalError);

    public ICommand SaveCommand { get; }

    public CandidateProfileEditViewModel(ICandidateProfileApiClient apiClient)
    {
        _apiClient = apiClient;
        SaveCommand = new Command(async () => await SaveAsync(), () => IsNotBusy);
    }

    public async Task LoadExistingProfileAsync(int id)
    {
        IsBusy = true;
        ClearErrors();

        try
        {
            var result = await _apiClient.GetProfileAsync(id);
            if (result.IsSuccess && result.Value != null)
            {
                Name = result.Value.Name;
                OcdId = result.Value.OcdId;
                Bio = result.Value.Bio;
                WebsiteUrl = result.Value.WebsiteUrl;
            }
            else
            {
                GeneralError = result.ErrorMessage;
            }
        }
        finally
        {
            IsBusy = false;
        }
    }

    private async Task SaveAsync()
    {
        if (IsBusy) return;

        ClearErrors();

        // Client-side quick validation
        bool hasClientErrors = false;
        if (string.IsNullOrWhiteSpace(Name))
        {
            NameError = "Candidate name is required.";
            hasClientErrors = true;
        }
        else if (Name.Trim().Length < 2 || Name.Trim().Length > 100)
        {
            NameError = "Candidate name must be between 2 and 100 characters.";
            hasClientErrors = true;
        }

        if (string.IsNullOrWhiteSpace(OcdId))
        {
            OcdIdError = "District identifier (OcdId) is required.";
            hasClientErrors = true;
        }
        else if (OcdId.Trim().Length < 5 || OcdId.Trim().Length > 100)
        {
            OcdIdError = "District identifier must be between 5 and 100 characters.";
            hasClientErrors = true;
        }

        if (!string.IsNullOrWhiteSpace(WebsiteUrl) && !Uri.TryCreate(WebsiteUrl.Trim(), UriKind.Absolute, out _))
        {
            WebsiteUrlError = "Website URL must be a valid absolute URL (e.g., https://example.com).";
            hasClientErrors = true;
        }

        if (hasClientErrors) return;

        IsBusy = true;
        ((Command)SaveCommand).ChangeCanExecute();

        try
        {
            var dto = new SaveCandidateProfileDto
            {
                Name = Name.Trim(),
                OcdId = OcdId.Trim(),
                Bio = string.IsNullOrWhiteSpace(Bio) ? null : Bio.Trim(),
                WebsiteUrl = string.IsNullOrWhiteSpace(WebsiteUrl) ? null : WebsiteUrl.Trim()
            };

            ApiResult result;
            if (IsEditMode)
            {
                result = await _apiClient.UpdateProfileAsync(ProfileId, dto);
            }
            else
            {
                result = await _apiClient.CreateProfileAsync(dto);
            }

            if (result.IsSuccess)
            {
                if (Shell.Current != null)
                {
                    await Shell.Current.GoToAsync("..");
                }
                return;
            }

            if (result.IsUnauthorized)
            {
                await SendToLoginAsync("Please log in to make changes.");
                return;
            }

            if (result.Problem?.Errors != null && result.Problem.Errors.Count > 0)
            {
                MapServerValidationErrors(result.Problem.Errors);
            }
            else
            {
                GeneralError = result.ErrorMessage;
            }
        }
        finally
        {
            IsBusy = false;
            ((Command)SaveCommand).ChangeCanExecute();
        }
    }

    private void MapServerValidationErrors(Dictionary<string, string[]> errors)
    {
        var unhandled = new List<string>();

        foreach (var (key, messages) in errors)
        {
            var text = string.Join(" ", messages);
            if (key.Equals("Name", StringComparison.OrdinalIgnoreCase))
            {
                NameError = text;
            }
            else if (key.Equals("OcdId", StringComparison.OrdinalIgnoreCase))
            {
                OcdIdError = text;
            }
            else if (key.Equals("Bio", StringComparison.OrdinalIgnoreCase))
            {
                BioError = text;
            }
            else if (key.Equals("WebsiteUrl", StringComparison.OrdinalIgnoreCase))
            {
                WebsiteUrlError = text;
            }
            else
            {
                unhandled.Add($"{key}: {text}");
            }
        }

        if (unhandled.Count > 0)
        {
            GeneralError = string.Join("\n", unhandled);
        }
    }

    private void ClearErrors()
    {
        NameError = null;
        OcdIdError = null;
        BioError = null;
        WebsiteUrlError = null;
        GeneralError = null;
        ErrorMessage = null;
    }
}
