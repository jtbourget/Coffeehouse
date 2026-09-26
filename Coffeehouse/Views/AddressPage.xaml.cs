using Coffeehouse.ViewModels;

namespace Coffeehouse.Views;

public partial class AddressPage : ContentPage
{
    public AddressPage(AddressViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is AddressViewModel viewModel)
        {
            await viewModel.InitializeAsync();
        }
    }

    private void OnSearchTextChanged(object sender, TextChangedEventArgs e)
    {
        if (BindingContext is AddressViewModel viewModel)
        {
            viewModel.SearchText = e.NewTextValue;
        }
    }
}
