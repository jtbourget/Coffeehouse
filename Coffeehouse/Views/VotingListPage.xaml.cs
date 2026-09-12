using Coffeehouse.ViewModels;

namespace Coffeehouse.Views;

public partial class VotingListPage : ContentPage
{
    private readonly VotingListViewModel _viewModel;

    public VotingListPage()
    {
        InitializeComponent();
        _viewModel = new VotingListViewModel();
        BindingContext = _viewModel;
    }

    public VotingListPage(int electionId)
    {
        InitializeComponent();
        _viewModel = new VotingListViewModel { ElectionId = electionId };
        BindingContext = _viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _ = _viewModel.LoadVotingListAsync();
    }
}
