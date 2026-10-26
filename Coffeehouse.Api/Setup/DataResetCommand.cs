using System.Text.Json;
using Coffeehouse.Api.Data;
using Coffeehouse.Api.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace Coffeehouse.Api.Setup;

public static class DataResetCommand
{
    public static async Task<int> RunAsync(string[] args, IConfiguration configuration, string environmentName, string contentRootPath)
    {
        if (environmentName != "Development")
        {
            Console.WriteLine("Reset is only allowed in the Development environment.");
            return 1;
        }

        var provider = configuration.GetValue<string>("DataSource:Provider") ?? "Json";

        Console.WriteLine($"Selected data source: {provider}");
        Console.WriteLine("This will replace all candidate profile data with the seed file. Type RESET Json or RESET SqlServer exactly to continue.");
        Console.Write("Confirmation: ");
        var confirmation = Console.ReadLine();

        if (confirmation != $"RESET {provider}")
        {
            Console.WriteLine("Reset cancelled.");
            return 0;
        }

        var seedFilePath = Path.Combine(contentRootPath, "Data", "CandidateProfiles.json");
        if (!File.Exists(seedFilePath))
        {
            Console.WriteLine($"Seed file not found: {seedFilePath}");
            return 1;
        }

        if (provider.Equals("Json", StringComparison.OrdinalIgnoreCase))
        {
            var workingFilePath = configuration.GetValue<string>("DataSource:JsonFilePath") ?? "Data/CandidateProfiles.json";
            workingFilePath = Path.GetFullPath(workingFilePath, contentRootPath);

            var backupPath = workingFilePath + "." + DateTime.Now.ToString("yyyyMMddHHmmss") + ".bak";
            Directory.CreateDirectory(Path.GetDirectoryName(workingFilePath)!);

            if (File.Exists(workingFilePath))
            {
                File.Copy(workingFilePath, backupPath, overwrite: false);
            }

            using var state = new JsonStoreState(workingFilePath);
            state.ReplaceWithSeed(seedFilePath);
            state.Dispose();

            Console.WriteLine($"JSON reset complete. Backup saved to {backupPath}");
        }
        else if (provider.Equals("SqlServer", StringComparison.OrdinalIgnoreCase))
        {
            var connectionString = configuration.GetConnectionString("Coffeehouse") ?? configuration.GetConnectionString("DefaultConnection");
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                Console.WriteLine("Database connection string is not configured. Run --setup-sql first.");
                return 1;
            }

            var adminConnectionString = SecretInput.ReadPassword("Admin SQL connection string for reset: ");
            await using var adminConnection = new SqlConnection(adminConnectionString);
            await adminConnection.OpenAsync();

            await using var transaction = adminConnection.BeginTransaction();
            try
            {
                var clearCommand = new SqlCommand("DELETE FROM [dbo].[CandidateProfiles];", adminConnection, transaction);
                await clearCommand.ExecuteNonQueryAsync();

                var reseedCommand = new SqlCommand("DBCC CHECKIDENT ('[dbo].[CandidateProfiles]', RESEED, 0);", adminConnection, transaction);
                await reseedCommand.ExecuteNonQueryAsync();

                var json = await File.ReadAllTextAsync(seedFilePath);
                var profiles = JsonSerializer.Deserialize<List<CandidateProfile>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new();
                foreach (var profile in profiles)
                {
                    var insert = new SqlCommand(@"
SET IDENTITY_INSERT [dbo].[CandidateProfiles] ON;
INSERT INTO [dbo].[CandidateProfiles] ([Id], [OcdId], [Name], [Bio], [WebsiteUrl], [CreatedAt], [IsVerified])
VALUES (@Id, @OcdId, @Name, @Bio, @WebsiteUrl, @CreatedAt, @IsVerified);
SET IDENTITY_INSERT [dbo].[CandidateProfiles] OFF;", adminConnection, transaction);

                    insert.Parameters.AddWithValue("@Id", profile.Id);
                    insert.Parameters.AddWithValue("@OcdId", profile.OcdId);
                    insert.Parameters.AddWithValue("@Name", profile.Name);
                    insert.Parameters.AddWithValue("@Bio", (object?)profile.Bio ?? DBNull.Value);
                    insert.Parameters.AddWithValue("@WebsiteUrl", (object?)profile.WebsiteUrl ?? DBNull.Value);
                    insert.Parameters.AddWithValue("@CreatedAt", profile.CreatedAt);
                    insert.Parameters.AddWithValue("@IsVerified", profile.IsVerified);
                    await insert.ExecuteNonQueryAsync();
                }

                await transaction.CommitAsync();
                Console.WriteLine("SQL Server reset complete.");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                Console.WriteLine($"SQL reset failed: {ex.Message}");
                return 1;
            }
        }

        return 0;
    }
}
