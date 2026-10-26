using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;
using System.Security.Cryptography;
using System.Text.Json;
using Coffeehouse.Api.Authentication;
using Coffeehouse.Api.Configuration;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration.UserSecrets;

namespace Coffeehouse.Api.Setup;

public static class DevelopmentBootstrap
{
    private const string SaPasswordEnvKey = "MSSQL_SA_PASSWORD";
    private const string SigningKeyConfigKey = "Authentication:SigningKey";
    private const string DockerRemediation =
        "Docker Desktop must be installed and running. Start it and rerun, or set " +
        "DataSource:AutoStartDocker=false and start the container yourself with `docker compose up -d sqlserver`.";

    public static string? FindRepoRoot(string startPath)
    {
        var directory = new DirectoryInfo(Path.GetFullPath(startPath));
        while (directory is not null)
        {
            if (File.Exists(Path.Combine(directory.FullName, "compose.yaml")))
            {
                return directory.FullName;
            }

            directory = directory.Parent;
        }

        return null;
    }

    public static Dictionary<string, string> ParseEnvFile(string content)
    {
        var values = new Dictionary<string, string>(StringComparer.Ordinal);
        foreach (var rawLine in content.Split('\n'))
        {
            var line = rawLine.Trim();
            if (line.Length == 0 || line.StartsWith('#'))
            {
                continue;
            }

            var separator = line.IndexOf('=');
            if (separator <= 0)
            {
                continue;
            }

            var key = line[..separator].Trim();
            var value = line[(separator + 1)..].Trim();
            if (value.Length >= 2 &&
                ((value[0] == '"' && value[^1] == '"') || (value[0] == '\'' && value[^1] == '\'')))
            {
                value = value[1..^1];
            }

            values[key] = value;
        }

        return values;
    }

    public static string GenerateSaPassword()
    {
        const string upper = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        const string lower = "abcdefghijklmnopqrstuvwxyz";
        const string digits = "0123456789";
        const string symbols = "!*-_+";
        const string all = upper + lower + digits + symbols;

        var chars = new char[24];
        for (var i = 0; i < chars.Length; i++)
        {
            chars[i] = all[RandomNumberGenerator.GetInt32(all.Length)];
        }

        chars[0] = upper[RandomNumberGenerator.GetInt32(upper.Length)];
        chars[1] = lower[RandomNumberGenerator.GetInt32(lower.Length)];
        chars[2] = digits[RandomNumberGenerator.GetInt32(digits.Length)];
        chars[3] = symbols[RandomNumberGenerator.GetInt32(symbols.Length)];
        RandomNumberGenerator.Shuffle(chars);
        return new string(chars);
    }

    public static string BuildSaConnectionString(string saPassword)
    {
        var builder = new SqlConnectionStringBuilder
        {
            DataSource = "tcp:127.0.0.1,14333",
            InitialCatalog = "Coffeehouse",
            UserID = "sa",
            Password = saPassword,
            TrustServerCertificate = true,
            Encrypt = true,
        };
        return builder.ConnectionString;
    }

    public static string EnsureSaPassword(string repoRoot)
    {
        var envPath = Path.Combine(repoRoot, ".env");
        if (File.Exists(envPath))
        {
            var existing = ParseEnvFile(File.ReadAllText(envPath));
            if (existing.TryGetValue(SaPasswordEnvKey, out var password) && !string.IsNullOrWhiteSpace(password))
            {
                return password;
            }
        }

        var generated = GenerateSaPassword();
        var prefix = File.Exists(envPath) && new FileInfo(envPath).Length > 0 &&
            !File.ReadAllText(envPath).EndsWith('\n')
            ? Environment.NewLine
            : string.Empty;
        File.AppendAllText(envPath, $"{prefix}{SaPasswordEnvKey}={generated}{Environment.NewLine}");
        Console.WriteLine($"Wrote a new SQL Server SA password to {envPath}");
        return generated;
    }

    public static void StartSqlContainer(string repoRoot, TimeSpan timeout = default)
    {
        if (timeout <= TimeSpan.Zero)
        {
            timeout = TimeSpan.FromMinutes(3);
        }

        Console.WriteLine("Starting the SQL Server container (docker compose up -d --wait sqlserver)...");

        var startInfo = new ProcessStartInfo
        {
            FileName = "docker",
            WorkingDirectory = repoRoot,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
        };
        startInfo.ArgumentList.Add("compose");
        startInfo.ArgumentList.Add("up");
        startInfo.ArgumentList.Add("-d");
        startInfo.ArgumentList.Add("--wait");
        startInfo.ArgumentList.Add("sqlserver");

        Process process;
        try
        {
            process = Process.Start(startInfo)
                ?? throw new InvalidOperationException($"Failed to start docker. {DockerRemediation}");
        }
        catch (Win32Exception ex)
        {
            Console.WriteLine($"Could not run docker: {ex.Message}. Continuing without auto-starting container.");
            return;
        }

        var stderr = process.StandardError.ReadToEndAsync();

        if (!process.WaitForExit(timeout))
        {
            try { process.Kill(entireProcessTree: true); } catch { /* best effort */ }
            Console.WriteLine($"docker compose did not finish within {timeout.TotalMinutes:0} minutes. {DockerRemediation}");
            return;
        }

        if (process.ExitCode != 0)
        {
            Console.WriteLine($"docker compose failed with exit code {process.ExitCode}: {stderr.Result}. {DockerRemediation}");
            return;
        }

        Console.WriteLine("SQL Server container is ready.");
    }

    public static string EnsureSigningKey(string userSecretsId) =>
        EnsureSigningKeyForId(userSecretsId).Key;

    internal static SigningKeyResult EnsureSigningKeyForId(string userSecretsId) =>
        EnsureSigningKeyAtPath(PathHelper.GetSecretsPathFromSecretsId(userSecretsId));

    internal static SigningKeyResult EnsureSigningKeyAtPath(string secretsFilePath)
    {
        var secrets = new Dictionary<string, JsonElement>();
        if (File.Exists(secretsFilePath))
        {
            var json = File.ReadAllText(secretsFilePath);
            if (!string.IsNullOrWhiteSpace(json))
            {
                secrets = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(json) ?? new();
            }
        }

        if (secrets.TryGetValue(SigningKeyConfigKey, out var existing) &&
            existing.ValueKind == JsonValueKind.String &&
            !string.IsNullOrWhiteSpace(existing.GetString()))
        {
            return new SigningKeyResult(existing.GetString()!, WasGenerated: false);
        }

        var key = JwtTokenService.GenerateRandomSigningKey();
        secrets[SigningKeyConfigKey] = JsonSerializer.SerializeToElement(key);
        Directory.CreateDirectory(Path.GetDirectoryName(secretsFilePath)!);
        File.WriteAllText(secretsFilePath, JsonSerializer.Serialize(secrets, new JsonSerializerOptions { WriteIndented = true }));
        return new SigningKeyResult(key, WasGenerated: true);
    }

    internal record SigningKeyResult(string Key, bool WasGenerated);

    public static void Run(WebApplicationBuilder builder)
    {
        var environment = builder.Environment;
        if (!environment.IsDevelopment())
        {
            return;
        }

        var configuration = builder.Configuration;
        var dataSourceOptions = configuration.GetSection(DataSourceOptions.SectionName).Get<DataSourceOptions>()
            ?? new DataSourceOptions();
        var authenticationOptions = configuration.GetSection(AuthenticationOptions.SectionName).Get<AuthenticationOptions>()
            ?? new AuthenticationOptions();

        if (dataSourceOptions.Provider.Equals("SqlServer", StringComparison.OrdinalIgnoreCase) &&
            dataSourceOptions.AutoStartDocker)
        {
            var repoRoot = FindRepoRoot(environment.ContentRootPath);
            if (repoRoot != null)
            {
                var saPassword = EnsureSaPassword(repoRoot);
                StartSqlContainer(repoRoot);

                var connStr = configuration.GetConnectionString("Coffeehouse") ?? configuration.GetConnectionString("DefaultConnection");
                if (string.IsNullOrWhiteSpace(connStr))
                {
                    configuration.AddInMemoryCollection(new Dictionary<string, string?>
                    {
                        ["ConnectionStrings:Coffeehouse"] = BuildSaConnectionString(saPassword),
                        ["ConnectionStrings:DefaultConnection"] = BuildSaConnectionString(saPassword),
                    });
                }
            }
        }

        if (authenticationOptions.Enabled &&
            authenticationOptions.AutoGenerateSigningKey &&
            string.IsNullOrWhiteSpace(authenticationOptions.SigningKey))
        {
            var secretsId = typeof(Program).Assembly.GetCustomAttribute<UserSecretsIdAttribute>()?.UserSecretsId;
            if (secretsId != null)
            {
                var result = EnsureSigningKeyForId(secretsId);
                configuration.AddInMemoryCollection(new Dictionary<string, string?>
                {
                    [SigningKeyConfigKey] = result.Key,
                });

                if (result.WasGenerated)
                {
                    Console.WriteLine("Generated a JWT signing key and saved it to user secrets.");
                }
            }
        }
    }
}
