namespace Coffeehouse
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(Views.ElectionsPage), typeof(Views.ElectionsPage));
            Routing.RegisterRoute(nameof(Views.BallotPage), typeof(Views.BallotPage));
            Routing.RegisterRoute(nameof(Views.CandidateDetailPage), typeof(Views.CandidateDetailPage));
            Routing.RegisterRoute(nameof(Views.VotingListDetailsPage), typeof(Views.VotingListDetailsPage));

            // Candidate profile management routes
            Routing.RegisterRoute(nameof(Views.CandidateProfilesListPage), typeof(Views.CandidateProfilesListPage));
            Routing.RegisterRoute(nameof(Views.CandidateProfileDetailPage), typeof(Views.CandidateProfileDetailPage));
            Routing.RegisterRoute(nameof(Views.CandidateProfileEditPage), typeof(Views.CandidateProfileEditPage));
            Routing.RegisterRoute(nameof(Views.LoginPage), typeof(Views.LoginPage));
        }
    }
}
