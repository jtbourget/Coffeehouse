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
#endif

            // Services
            var baseAddress = DeviceInfo.Platform == DevicePlatform.Android ? "http://10.0.2.2:5032" : "http://localhost:5032";
            builder.Services.AddHttpClient("CoffeehouseApi", client =>
            {
                client.BaseAddress = new Uri(baseAddress);
            });
            builder.Services.AddSingleton<Services.IApiService, Services.ApiService>();

            // ViewModels
            builder.Services.AddTransient<ViewModels.AddressViewModel>();
            builder.Services.AddTransient<ViewModels.BallotViewModel>();
            builder.Services.AddTransient<ViewModels.CandidateDetailViewModel>();
            builder.Services.AddTransient<ViewModels.ElectionsViewModel>();
            builder.Services.AddTransient<ViewModels.VotingListViewModel>();

            // Pages
            builder.Services.AddTransient<MainPage>();
            builder.Services.AddTransient<Views.AddressPage>();
            builder.Services.AddTransient<Views.BallotPage>();
            builder.Services.AddTransient<Views.CandidateDetailPage>();
            builder.Services.AddTransient<Views.ElectionsPage>();
            builder.Services.AddTransient<Views.VotingListPage>();
            builder.Services.AddTransient<Views.VotingListDetailsPage>();

            return builder.Build();
        }
    }
}
