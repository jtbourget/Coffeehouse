using Coffeehouse.ViewModels;

namespace Coffeehouse.Views;

public partial class CandidateDetailPage : ContentPage
{
    private readonly CandidateDetailViewModel _viewModel;

    public CandidateDetailPage(int candidateId, int contestId)
    {
        InitializeComponent();
        _viewModel = new CandidateDetailViewModel();
        BindingContext = _viewModel;
        
        // Fire and forget the loading of candidate details
        _ = _viewModel.LoadCandidateAsync(candidateId, contestId);
    }
}
