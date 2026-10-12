using Coffeehouse.Api.Data;
using Coffeehouse.Api.Repositories;
using Coffeehouse.Api.Services;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using Microsoft.EntityFrameworkCore;

// Initialize the web application builder with the provided command-line arguments.
var builder = WebApplication.CreateBuilder(args);

// --- Service Registration ---

// Register controllers with SystemTextJsonValidationMetadataProvider so validation error keys match JSON property names.
builder.Services.AddControllers(options =>
{
    options.ModelMetadataDetailsProviders.Add(new SystemTextJsonValidationMetadataProvider());
});

// Register OpenAPI services to generate API documentation automatically.
builder.Services.AddOpenApi();

// Register the Entity Framework Core database context.
// Configures SQL Server LocalDB with connection string from configuration.
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"))
        .ConfigureWarnings(w => w.Ignore(Microsoft.EntityFrameworkCore.Diagnostics.RelationalEventId.PendingModelChangesWarning)));

// ProblemDetails service registration for RFC 7807 responses
builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<Coffeehouse.Api.Infrastructure.GlobalExceptionHandler>();

// --- Dependency Injection Registrations ---

// Repositories registered as Singleton
builder.Services.AddSingleton<IAddressRepository, SqlAddressRepository>();
builder.Services.AddSingleton<IElectionRepository, SqlElectionRepository>();
builder.Services.AddSingleton<IContestRepository, SqlContestRepository>();
builder.Services.AddSingleton<ICandidateRepository, SqlCandidateRepository>();
builder.Services.AddSingleton<ICandidateProfileRepository, SqlCandidateProfileRepository>();
builder.Services.AddSingleton<IFavoriteCandidateRepository, SqlFavoriteCandidateRepository>();
builder.Services.AddSingleton<IFavoritesRepository, SqlFavoritesRepository>();

// Services registered as Scoped
builder.Services.AddScoped<IAddressService, AddressService>();
builder.Services.AddScoped<IElectionService, ElectionService>();
builder.Services.AddScoped<IContestService, ContestService>();
builder.Services.AddScoped<ICandidateService, CandidateService>();
builder.Services.AddScoped<ICandidateProfileService, CandidateProfileService>();
builder.Services.AddScoped<IFavoritesService, FavoritesService>();

// Mappers registered as Scoped
builder.Services.AddScoped<IAddressMapper, AddressMapper>();
builder.Services.AddScoped<IElectionMapper, ElectionMapper>();
builder.Services.AddScoped<IContestMapper, ContestMapper>();
builder.Services.AddScoped<ICandidateMapper, CandidateMapper>();
builder.Services.AddScoped<ICandidateProfileMapper, CandidateProfileMapper>();
builder.Services.AddScoped<IFavoriteCandidateMapper, FavoriteCandidateMapper>();
builder.Services.AddScoped<IFavoritesMapper, FavoritesMapper>();

// HTTP client services
builder.Services.AddHttpClient<Coffeehouse.Api.Services.GooglePlacesService>();
builder.Services.AddHttpClient<Coffeehouse.Api.Services.GeocodioService>();

// Build the web application pipeline.
var app = builder.Build();

// --- Database Initialization ---
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    SeedData.Initialize(context);
}

// --- HTTP Pipeline Configuration ---

// Enable OpenAPI endpoints (available in development and testing).
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

// Add middleware for authorization.
app.UseAuthorization();

// Add global exception handler middleware for unhandled exceptions
app.UseExceptionHandler();

// Map controller endpoints to the request pipeline.
app.MapControllers();

// Start the application and begin listening for incoming HTTP requests.
app.Run();
