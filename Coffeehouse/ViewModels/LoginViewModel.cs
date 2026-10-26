using System.Windows.Input;
using Coffeehouse.Services;

namespace Coffeehouse.ViewModels;

[QueryProperty(nameof(Message), "message")]
public class LoginViewModel : ViewModelBase
{
    private readonly IAuthService _authService;
    private string _username = string.Empty;
    private string _password = string.Empty;
    private string? _message;

    public string Username
    {
        get => _username;
        set { if (_username != value) { _username = value; OnPropertyChanged(); } }
    }

    public string Password
    {
        get => _password;
        set { if (_password != value) { _password = value; OnPropertyChanged(); } }
    }

    public string? Message
    {
        get => _message;
        set { if (_message != value) { _message = value; OnPropertyChanged(); OnPropertyChanged(nameof(HasMessage)); } }
    }

    public bool HasMessage => !string.IsNullOrWhiteSpace(_message);

    public ICommand LoginCommand { get; }

    public LoginViewModel(IAuthService authService)
    {
        _authService = authService;

#if DEBUG
        // Facilitate fast one-tap testing in debug mode
        _username = "student";
        _password = "Student123!";
#endif

        LoginCommand = new Command(async () => await ExecuteLoginAsync(), () => IsNotBusy);
    }

    private async Task ExecuteLoginAsync()
    {
        if (IsBusy) return;

        if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password))
        {
            ErrorMessage = "Username and password are required.";
            return;
        }

        IsBusy = true;
        ErrorMessage = null;
        ((Command)LoginCommand).ChangeCanExecute();

        try
        {
            var result = await _authService.LoginAsync(Username.Trim(), Password);
            if (result.IsSuccess)
            {
                Password = string.Empty;
                if (Shell.Current != null)
                {
                    await Shell.Current.GoToAsync("..");
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
            ((Command)LoginCommand).ChangeCanExecute();
        }
    }
}
