using Coffeehouse.ViewModels;

namespace Coffeehouse.Views;

public partial class ElectionsPage : ContentPage
{
    public ElectionsPage(ElectionsViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        
        if (BindingContext is ElectionsViewModel viewModel)
        {
            await viewModel.InitializeAsync();
        }
    }
}
