using System.Data;
using System.Diagnostics;
using Microsoft.Data.SqlClient;

namespace Coffeehouse.Api.Setup;

public static class SqlDatabaseSetup
{
    public static async Task<int> RunAsync(string projectPath)
    {
        Console.WriteLine("This command creates the Coffeehouse database, applies migrations, and creates a restricted app login.");
        Console.WriteLine();

        var adminConnectionString = SecretInput.ReadPassword("Admin SQL connection string (will not be saved): ");
        var appPassword = SecretInput.ReadPassword("Password for the new CoffeehouseApp SQL login (min 8 chars, strong): ");
        var confirmPassword = SecretInput.ReadPassword("Confirm app login password: ");

        if (appPassword != confirmPassword)
        {
            Console.WriteLine("Passwords do not match.");
            return 1;
        }

        try
        {
            await using var adminConnection = new SqlConnection(adminConnectionString);
            await adminConnection.OpenAsync();

            await CreateDatabaseAsync(adminConnection);
            await ApplyMigrationsAsync(projectPath, adminConnectionString);
            await CreateAppLoginAsync(adminConnection, appPassword);
            await VerifyAppConnectionAsync(projectPath, adminConnectionString, appPassword);

            Console.WriteLine();
            Console.WriteLine("SQL setup complete. Runtime connection string saved to user secrets.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Setup failed: {ex.Message}");
            return 1;
        }

        return 0;
    }

    private static async Task CreateDatabaseAsync(SqlConnection adminConnection)
    {
        var command = adminConnection.CreateCommand();
        command.CommandText = "IF DB_ID('Coffeehouse') IS NULL CREATE DATABASE [Coffeehouse];";
        await command.ExecuteNonQueryAsync();
        Console.WriteLine("Database Coffeehouse verified/created.");
    }

    private static async Task ApplyMigrationsAsync(string projectPath, string adminConnectionString)
    {
        var builder = new SqlConnectionStringBuilder(adminConnectionString) { InitialCatalog = "Coffeehouse" };

        var startInfo = new ProcessStartInfo
        {
            FileName = "dotnet",
            WorkingDirectory = projectPath,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
        };
        startInfo.ArgumentList.Add("ef");
        startInfo.ArgumentList.Add("database");
        startInfo.ArgumentList.Add("update");
        startInfo.ArgumentList.Add("--no-build");
        startInfo.ArgumentList.Add("--connection");
        startInfo.ArgumentList.Add(builder.ConnectionString);

        var process = Process.Start(startInfo) ?? throw new InvalidOperationException("Failed to start dotnet ef.");
        await process.WaitForExitAsync();
        if (process.ExitCode != 0)
        {
            var error = await process.StandardError.ReadToEndAsync();
            throw new InvalidOperationException($"Migration failed: {error}");
        }

        Console.WriteLine("Migrations applied.");
    }

    private static async Task CreateAppLoginAsync(SqlConnection adminConnection, string appPassword)
    {
        var statements = $@"
IF NOT EXISTS (SELECT * FROM sys.sql_logins WHERE name = 'CoffeehouseApp')
BEGIN
    DECLARE @loginSql nvarchar(max) =
        N'CREATE LOGIN [CoffeehouseApp] WITH PASSWORD = N''' +
        REPLACE(@password, N'''', N'''''') + N''';';
    EXEC sys.sp_executesql @loginSql;
END;

USE [Coffeehouse];

IF NOT EXISTS (SELECT * FROM sys.database_principals WHERE name = 'CoffeehouseApp')
    CREATE USER [CoffeehouseApp] FOR LOGIN [CoffeehouseApp];

GRANT SELECT, INSERT, UPDATE, DELETE ON SCHEMA::dbo TO [CoffeehouseApp];
";

        var command = adminConnection.CreateCommand();
        command.CommandText = statements;
        command.Parameters.Add(new SqlParameter("@password", SqlDbType.NVarChar) { Value = appPassword });
        await command.ExecuteNonQueryAsync();
        Console.WriteLine("App login and user permissions configured.");
    }

    private static async Task VerifyAppConnectionAsync(string projectPath, string adminConnectionString, string appPassword)
    {
        var builder = new SqlConnectionStringBuilder(adminConnectionString)
        {
            InitialCatalog = "Coffeehouse",
            UserID = "CoffeehouseApp",
            Password = appPassword,
        };

        await using var connection = new SqlConnection(builder.ConnectionString);
        await connection.OpenAsync();

        var values = new Dictionary<string, object>
        {
            ["ConnectionStrings:Coffeehouse"] = builder.ConnectionString,
            ["ConnectionStrings:DefaultConnection"] = builder.ConnectionString,
        };
        await UserSecretsWriter.SetAsync(projectPath, values);
    }
}
