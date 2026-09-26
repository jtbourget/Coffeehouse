using Coffeehouse.ViewModels;

namespace Coffeehouse.Views;

[QueryProperty(nameof(ElectionId), "ElectionId")]
public partial class VotingListDetailsPage : ContentPage
{
    private readonly VotingListViewModel _viewModel;

    public int ElectionId 
    { 
        set 
        { 
            _viewModel.ElectionId = value; 
        } 
    }

    public VotingListDetailsPage(VotingListViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.LoadVotingListAsync();
    }
}
