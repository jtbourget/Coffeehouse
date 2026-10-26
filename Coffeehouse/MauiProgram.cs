using Coffeehouse.Services;
using Coffeehouse.ViewModels;
using Coffeehouse.Views;
using Microsoft.Extensions.Logging;

namespace Coffeehouse
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
            builder.Logging.AddDebug();
            builder.Logging.AddConsole();
            builder.Logging.SetMinimumLevel(LogLevel.Debug);
#else
            builder.Logging.AddConsole();
            builder.Logging.SetMinimumLevel(LogLevel.Information);
#endif

            // --- Authentication & Token Storage Services ---
            builder.Services.AddSingleton<ITokenStore, SecureTokenStore>();
            builder.Services.AddTransient<AuthTokenHandler>();

            // --- HTTP Client Registrations ---
            var baseUri = new Uri(ApiSettings.BaseAddress);

            // Auth client (without AuthTokenHandler)
            var authHttpBuilder = builder.Services.AddHttpClient<IAuthService, AuthService>(client =>
            {
                client.BaseAddress = baseUri;
                client.Timeout = ApiSettings.RequestTimeout;
            });

            // Candidate profile client (with AuthTokenHandler)
            var candidateHttpBuilder = builder.Services.AddHttpClient<ICandidateProfileApiClient, CandidateProfileApiClient>(client =>
            {
                client.BaseAddress = baseUri;
                client.Timeout = ApiSettings.RequestTimeout;
            }).AddHttpMessageHandler<AuthTokenHandler>();

            // Legacy CoffeehouseApi client for ballot and address features
            var legacyHttpBuilder = builder.Services.AddHttpClient("CoffeehouseApi", client =>
            {
                client.BaseAddress = baseUri;
                client.Timeout = ApiSettings.RequestTimeout;
            });

#if DEBUG
            authHttpBuilder.ConfigurePrimaryHttpMessageHandler(() => new DevCertificateHandler());
            candidateHttpBuilder.ConfigurePrimaryHttpMessageHandler(() => new DevCertificateHandler());
            legacyHttpBuilder.ConfigurePrimaryHttpMessageHandler(() => new DevCertificateHandler());
#endif

            builder.Services.AddSingleton<IApiService, ApiService>();

            // --- ViewModels ---
            builder.Services.AddTransient<AddressViewModel>();
            builder.Services.AddTransient<BallotViewModel>();
            builder.Services.AddTransient<CandidateDetailViewModel>();
            builder.Services.AddTransient<ElectionsViewModel>();
            builder.Services.AddTransient<VotingListViewModel>();

            // Candidate profile management ViewModels
            builder.Services.AddTransient<LoginViewModel>();
            builder.Services.AddTransient<CandidateProfilesListViewModel>();
            builder.Services.AddTransient<CandidateProfileDetailViewModel>();
            builder.Services.AddTransient<CandidateProfileEditViewModel>();

            // --- Pages ---
            builder.Services.AddTransient<MainPage>();
            builder.Services.AddTransient<AddressPage>();
            builder.Services.AddTransient<BallotPage>();
            builder.Services.AddTransient<CandidateDetailPage>();
            builder.Services.AddTransient<ElectionsPage>();
            builder.Services.AddTransient<VotingListPage>();
            builder.Services.AddTransient<VotingListDetailsPage>();

            // Candidate profile management Pages
            builder.Services.AddTransient<LoginPage>();
            builder.Services.AddTransient<CandidateProfilesListPage>();
            builder.Services.AddTransient<CandidateProfileDetailPage>();
            builder.Services.AddTransient<CandidateProfileEditPage>();

            return builder.Build();
        }
    }
}
