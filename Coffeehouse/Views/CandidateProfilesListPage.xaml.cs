using Coffeehouse.ViewModels;

namespace Coffeehouse.Views;

public partial class CandidateProfilesListPage : ContentPage
{
    private readonly CandidateProfilesListViewModel _viewModel;

    public CandidateProfilesListPage(CandidateProfilesListViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = _viewModel = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.RefreshStateAsync();
    }
}
