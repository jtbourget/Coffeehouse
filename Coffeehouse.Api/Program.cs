using Coffeehouse.Api.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// --- Service Registration ---

// Register controllers for handling API requests
builder.Services.AddControllers();

// Register OpenAPI/Swagger for API documentation in development
builder.Services.AddOpenApi();

// Register the EF Core database context with SQL Server LocalDB
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// --- Database Initialization ---
// Create database and seed sample data on startup
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    SeedData.Initialize(context);
}

// --- HTTP Pipeline Configuration ---

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseAuthorization();
app.MapControllers();

app.Run();
