namespace Coffeehouse
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(Views.ElectionsPage), typeof(Views.ElectionsPage));
            Routing.RegisterRoute(nameof(Views.BallotPage), typeof(Views.BallotPage));
        }
    }
}
