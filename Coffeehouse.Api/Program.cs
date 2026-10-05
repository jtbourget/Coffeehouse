using Coffeehouse.Api.Data;
using Microsoft.EntityFrameworkCore;

// Initialize the web application builder with the provided command-line arguments.
var builder = WebApplication.CreateBuilder(args);

// --- Service Registration ---

// Register controllers for handling API requests and routing.
builder.Services.AddControllers();

// Register OpenAPI/Swagger services to generate API documentation automatically in development environments.
builder.Services.AddOpenApi();

// Register the Entity Framework Core database context.
// This configures the application to use SQL Server LocalDB with the connection string defined in the application configuration.
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<Coffeehouse.Api.Infrastructure.GlobalExceptionHandler>();

// Register Repositories and Services
builder.Services.AddScoped<Coffeehouse.Api.Repositories.IAddressRepository, Coffeehouse.Api.Repositories.SqlAddressRepository>();
builder.Services.AddScoped<Coffeehouse.Api.Services.IAddressMapper, Coffeehouse.Api.Services.AddressMapper>();
builder.Services.AddScoped<Coffeehouse.Api.Repositories.IElectionRepository, Coffeehouse.Api.Repositories.SqlElectionRepository>();
builder.Services.AddScoped<Coffeehouse.Api.Services.IElectionMapper, Coffeehouse.Api.Services.ElectionMapper>();
builder.Services.AddScoped<Coffeehouse.Api.Repositories.IContestRepository, Coffeehouse.Api.Repositories.SqlContestRepository>();
builder.Services.AddScoped<Coffeehouse.Api.Services.IContestMapper, Coffeehouse.Api.Services.ContestMapper>();
builder.Services.AddScoped<Coffeehouse.Api.Repositories.ICandidateRepository, Coffeehouse.Api.Repositories.SqlCandidateRepository>();
builder.Services.AddScoped<Coffeehouse.Api.Services.ICandidateMapper, Coffeehouse.Api.Services.CandidateMapper>();
builder.Services.AddScoped<Coffeehouse.Api.Repositories.ICandidateProfileRepository, Coffeehouse.Api.Repositories.SqlCandidateProfileRepository>();
builder.Services.AddScoped<Coffeehouse.Api.Services.ICandidateProfileMapper, Coffeehouse.Api.Services.CandidateProfileMapper>();
builder.Services.AddScoped<Coffeehouse.Api.Repositories.IFavoriteCandidateRepository, Coffeehouse.Api.Repositories.SqlFavoriteCandidateRepository>();
builder.Services.AddScoped<Coffeehouse.Api.Services.IFavoriteCandidateMapper, Coffeehouse.Api.Services.FavoriteCandidateMapper>();

builder.Services.AddHttpClient<Coffeehouse.Api.Services.GooglePlacesService>();
builder.Services.AddHttpClient<Coffeehouse.Api.Services.GeocodioService>();

// Build the web application pipeline.
var app = builder.Build();

// --- Database Initialization ---
// Create a new dependency injection scope to resolve scoped services.
// This is necessary because DbContext is registered as a scoped service by default.
using (var scope = app.Services.CreateScope())
{
    // Retrieve the database context instance from the service provider.
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    
    // Call the database initialization logic to ensure the database is created and seeded with sample data.
    SeedData.Initialize(context);
}

// --- HTTP Pipeline Configuration ---

// Enable OpenAPI endpoints only in the development environment to prevent exposing API details in production.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

// Add middleware for authorization.
app.UseAuthorization();

// Add global exception handler
app.UseExceptionHandler();

// Map controller endpoints to the request pipeline.
app.MapControllers();

// Start the application and begin listening for incoming HTTP requests.
app.Run();

