using Coffeehouse.ViewModels;

namespace Coffeehouse.Views;

public partial class CandidateProfileEditPage : ContentPage
{
    public CandidateProfileEditPage(CandidateProfileEditViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
