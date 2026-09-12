using Coffeehouse.ViewModels;

namespace Coffeehouse.Views;

public partial class ElectionsPage : ContentPage
{
    public ElectionsPage()
    {
        InitializeComponent();
        BindingContext = new ElectionsViewModel();
    }
}
