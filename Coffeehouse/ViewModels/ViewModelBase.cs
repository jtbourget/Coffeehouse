using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Coffeehouse.ViewModels;

public abstract class ViewModelBase : INotifyPropertyChanged
{
    private bool _isBusy;
    private string? _errorMessage;

    public bool IsBusy
    {
        get => _isBusy;
        set
        {
            if (_isBusy != value)
            {
                _isBusy = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsNotBusy));
            }
        }
    }

    public bool IsNotBusy => !IsBusy;

    public string? ErrorMessage
    {
        get => _errorMessage;
        set
        {
            if (_errorMessage != value)
            {
                _errorMessage = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(HasError));
            }
        }
    }

    public bool HasError => !string.IsNullOrWhiteSpace(_errorMessage);

    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public async Task SendToLoginAsync(string message = "Please log in to make changes.")
    {
        if (Shell.Current != null)
        {
            await Shell.Current.GoToAsync($"LoginPage?message={Uri.EscapeDataString(message)}");
        }
    }
}
