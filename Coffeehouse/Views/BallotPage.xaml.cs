using Coffeehouse.ViewModels;

namespace Coffeehouse.Views;

public partial class BallotPage : ContentPage, IQueryAttributable
{
    private readonly BallotViewModel _viewModel;

    public BallotPage(BallotViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
    }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue("id", out var idObj) && int.TryParse(idObj?.ToString(), out var id))
        {
            _viewModel.ElectionId = id;
            _viewModel.LoadBallotCommand.Execute(null);
        }
    }
}
