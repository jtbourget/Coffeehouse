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
        /// <summary>
        /// Initializes a new instance of the <see cref="App"/> class.
        /// </summary>
        public App()
        {
            // Initialize the user interface components.
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