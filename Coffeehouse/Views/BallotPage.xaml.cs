using Coffeehouse.ViewModels;

namespace Coffeehouse.Views;

[QueryProperty(nameof(ElectionId), "id")]
public partial class BallotPage : ContentPage
{
    private readonly BallotViewModel _viewModel;

    public int ElectionId 
    { 
        set 
        { 
            _viewModel.ElectionId = value; 
        } 
    }

    public BallotPage()
    {
        InitializeComponent();
        _viewModel = new BallotViewModel();
        BindingContext = _viewModel;
    }

    public BallotPage(int electionId) : this()
    {
        ElectionId = electionId;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _viewModel.LoadBallotCommand.Execute(null);
    }
}
