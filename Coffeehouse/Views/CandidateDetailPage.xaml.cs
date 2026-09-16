using Coffeehouse.ViewModels;

namespace Coffeehouse.Views;

public partial class CandidateDetailPage : ContentPage
{
    public CandidateDetailPage(CandidateDetailViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        
        if (BindingContext is CandidateDetailViewModel viewModel)
        {
            await viewModel.LoadCandidateAsync(viewModel.CandidateId, viewModel.ContestId);
        }
    }
}
