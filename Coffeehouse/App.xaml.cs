using Coffeehouse.Views;

namespace Coffeehouse
{
    /// <summary>
    /// Application entry point. Sets up the main navigation structure.
    /// Uses NavigationPage with AddressPage as the starting page,
    /// matching the todolist sample's navigation pattern.
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Creates the main application window using AppShell for tabbed navigation.
        /// </summary>
        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new AppShell());
        }
    }
}