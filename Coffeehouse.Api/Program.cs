using System.Text;
using Coffeehouse.Api.Authentication;
using Coffeehouse.Api.Configuration;
using Coffeehouse.Api.Data;
using Coffeehouse.Api.Models;
using Coffeehouse.Api.Repositories;
using Coffeehouse.Api.Services;
using Coffeehouse.Api.Setup;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Local setup command modes must run before the full web application pipeline is configured.
if (SetupCommandRunner.IsCommandMode(args, out var command))
{
    var projectPath = Path.GetDirectoryName(typeof(Program).Assembly.Location)!;
    if (projectPath.Contains("bin"))
    {
        projectPath = Directory.GetCurrentDirectory();
    }

    var result = command switch
    {
        "setup-sql" => await SqlDatabaseSetup.RunAsync(projectPath),
        "setup-auth" => await DemoAuthSetup.RunAsync(projectPath),
        "hash-password" => PasswordHashCommand.Run(),
        "reset-data" => await DataResetCommand.RunAsync(args, builder.Configuration, builder.Environment.EnvironmentName, builder.Environment.ContentRootPath),
        _ => RunWebApplication(builder, args),
    };

    return result;
}

return RunWebApplication(builder, args);

static int RunWebApplication(WebApplicationBuilder builder, string[] args)
{
    var services = builder.Services;
    var configuration = builder.Configuration;
    var environment = builder.Environment;

    // In Development, start Docker and fill in secrets before options are read below.
    DevelopmentBootstrap.Run(builder);

    // --- Configuration Options ---
    var dataSourceOptions = configuration.GetSection(DataSourceOptions.SectionName).Get<DataSourceOptions>()
        ?? new DataSourceOptions();
    services.Configure<DataSourceOptions>(configuration.GetSection(DataSourceOptions.SectionName));

    var authenticationOptions = configuration.GetSection(AuthenticationOptions.SectionName).Get<AuthenticationOptions>()
        ?? new AuthenticationOptions();
    services.Configure<AuthenticationOptions>(configuration.GetSection(AuthenticationOptions.SectionName));

    if (!environment.IsDevelopment() && !authenticationOptions.Enabled)
    {
        throw new InvalidOperationException("Authentication cannot be disabled outside of the Development environment.");
    }

    // --- Service Registration ---

    // Register controllers with SystemTextJsonValidationMetadataProvider so validation error keys match JSON property names.
    services.AddControllers(options =>
    {
        options.ModelMetadataDetailsProviders.Add(new SystemTextJsonValidationMetadataProvider());
    });

    // Configure HTTP request/response logging
    services.AddHttpLogging(logging =>
    {
        logging.LoggingFields = HttpLoggingFields.RequestMethod
            | HttpLoggingFields.RequestPath
            | HttpLoggingFields.ResponseStatusCode
            | HttpLoggingFields.Duration;
    });

    // Register OpenAPI services to generate API documentation automatically.
    services.AddOpenApi();

    // SQL Connection String
    var connectionString = configuration.GetConnectionString("Coffeehouse")
        ?? configuration.GetConnectionString("DefaultConnection")
        ?? "Server=(localdb)\\MSSQLLocalDB;Database=CoffeehouseDb;Trusted_Connection=True;TrustServerCertificate=True;";

    // Register the Entity Framework Core database context
    services.AddDbContext<AppDbContext>(options =>
        options.UseSqlServer(connectionString, sqlOptions =>
        {
            sqlOptions.MigrationsAssembly(typeof(Program).Assembly.FullName);
            sqlOptions.EnableRetryOnFailure();
        }).ConfigureWarnings(w => w.Ignore(Microsoft.EntityFrameworkCore.Diagnostics.RelationalEventId.PendingModelChangesWarning)));

    // ProblemDetails service registration for RFC 7807 responses
    services.AddProblemDetails();
    services.AddExceptionHandler<Coffeehouse.Api.Infrastructure.GlobalExceptionHandler>();

    // --- Switchable Repository Registration ---
    var workingJsonPath = Path.GetFullPath(dataSourceOptions.JsonFilePath, environment.ContentRootPath);
    Directory.CreateDirectory(Path.GetDirectoryName(workingJsonPath)!);
    var seedFilePath = Path.Combine(environment.ContentRootPath, "Data", "CandidateProfiles.json");

    services.AddSingleton<JsonStoreState>(_ =>
    {
        var state = new JsonStoreState(workingJsonPath);
        state.InitializeFromSeed(seedFilePath);
        return state;
    });

    if (dataSourceOptions.Provider.Equals("SqlServer", StringComparison.OrdinalIgnoreCase))
    {
        services.AddSingleton<ICandidateProfileRepository, SqlCandidateProfileRepository>();
    }
    else if (dataSourceOptions.Provider.Equals("Json", StringComparison.OrdinalIgnoreCase))
    {
        services.AddSingleton<ICandidateProfileRepository, JsonCandidateProfileRepository>();
    }
    else
    {
        throw new InvalidOperationException($"DataSource:Provider must be 'Json' or 'SqlServer'. Found: {dataSourceOptions.Provider}");
    }

    // Other repositories registered as Singleton
    services.AddSingleton<IAddressRepository, SqlAddressRepository>();
    services.AddSingleton<IElectionRepository, SqlElectionRepository>();
    services.AddSingleton<IContestRepository, SqlContestRepository>();
    services.AddSingleton<ICandidateRepository, SqlCandidateRepository>();
    services.AddSingleton<IFavoriteCandidateRepository, SqlFavoriteCandidateRepository>();
    services.AddSingleton<IFavoritesRepository, SqlFavoritesRepository>();

    // Services registered as Scoped
    services.AddScoped<IAddressService, AddressService>();
    services.AddScoped<IElectionService, ElectionService>();
    services.AddScoped<IContestService, ContestService>();
    services.AddScoped<ICandidateService, CandidateService>();
    services.AddScoped<ICandidateProfileService, CandidateProfileService>();
    services.AddScoped<IFavoritesService, FavoritesService>();

    // Mappers registered as Scoped
    services.AddScoped<IAddressMapper, AddressMapper>();
    services.AddScoped<IElectionMapper, ElectionMapper>();
    services.AddScoped<IContestMapper, ContestMapper>();
    services.AddScoped<ICandidateMapper, CandidateMapper>();
    services.AddScoped<ICandidateProfileMapper, CandidateProfileMapper>();
    services.AddScoped<IFavoriteCandidateMapper, FavoriteCandidateMapper>();
    services.AddScoped<IFavoritesMapper, FavoritesMapper>();

    // HTTP client services
    services.AddHttpClient<Coffeehouse.Api.Services.GooglePlacesService>();
    services.AddHttpClient<Coffeehouse.Api.Services.GeocodioService>();

    // --- Authentication and Authorization ---
    if (authenticationOptions.Enabled)
    {
        if (string.IsNullOrWhiteSpace(authenticationOptions.SigningKey))
        {
            throw new InvalidOperationException("Authentication:SigningKey is required when authentication is enabled. Run --setup-auth first or enable AutoGenerateSigningKey.");
        }

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = authenticationOptions.Issuer,
                    ValidAudience = authenticationOptions.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authenticationOptions.SigningKey)),
                    ClockSkew = TimeSpan.FromSeconds(30),
                };
            });
    }
    else
    {
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer();
    }

    services.AddAuthorization(options =>
    {
        options.AddPolicy("CandidateProfileWrite", policy =>
        {
            if (authenticationOptions.Enabled)
            {
                policy.RequireAuthenticatedUser();
            }
            else
            {
                policy.RequireAssertion(_ => true);
            }
        });
    });

    // Authentication services for demo users
    services.AddSingleton<Microsoft.AspNetCore.Identity.PasswordHasher<DemoUser>>();
    services.Configure<DemoUserStoreOptions>(configuration.GetSection("Authentication:DemoUsers"));
    services.AddSingleton<DemoUserStore>();
    services.AddSingleton<JwtTokenService>();
    services.AddSingleton(new LoginRateLimiter(new RateLimitOptions()));

    // Build the web application pipeline.
    var app = builder.Build();

    // --- Startup Logging ---
    app.Logger.LogInformation("Active data source provider: {Provider}", dataSourceOptions.Provider);
    app.Logger.LogInformation("Authentication enabled: {AuthEnabled}", authenticationOptions.Enabled);

    // --- Database Initialization / Migrations ---
    if (dataSourceOptions.Provider.Equals("SqlServer", StringComparison.OrdinalIgnoreCase))
    {
        try
        {
            using var scope = app.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            if (dataSourceOptions.MigrateOnStartup)
            {
                app.Logger.LogInformation("Applying database migrations and verifying seed data...");
                context.Database.Migrate();
            }
            SeedData.Initialize(context);
            app.Logger.LogInformation("Database initialization completed successfully.");
        }
        catch (Exception ex)
        {
            app.Logger.LogWarning(ex, "Could not initialize SQL Server database on startup. Ensure SQL Server is reachable if using SqlServer provider.");
        }
    }

    // --- HTTP Pipeline Configuration ---
    app.UseHttpLogging();

    if (app.Environment.IsDevelopment())
    {
        app.MapOpenApi();
    }

    app.UseExceptionHandler();
    app.UseHttpsRedirection();

    if (authenticationOptions.Enabled)
    {
        app.UseAuthentication();
    }

    app.UseAuthorization();
    app.MapControllers();

    app.Logger.LogInformation("Coffeehouse API starting on environment: {Environment}", app.Environment.EnvironmentName);
    app.Run();

    return 0;
}
