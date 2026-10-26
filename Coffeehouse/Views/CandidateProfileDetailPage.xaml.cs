using Coffeehouse.ViewModels;

namespace Coffeehouse.Views;

public partial class CandidateProfileDetailPage : ContentPage
{
    public CandidateProfileDetailPage(CandidateProfileDetailViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
