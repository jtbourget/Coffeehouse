using Coffeehouse.ViewModels;

namespace Coffeehouse.Views;

public partial class VotingListPage : ContentPage
{
    private readonly VotingListViewModel _viewModel;

    public VotingListPage(VotingListViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.LoadElectionsAsync();
    }
}
