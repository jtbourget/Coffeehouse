using System.Diagnostics;
using System.Text.Json;

namespace Coffeehouse.Api.Setup;

public static class UserSecretsWriter
{
    public static async Task SetAsync(string projectPath, Dictionary<string, object> values)
    {
        var startInfo = new ProcessStartInfo
        {
            FileName = "dotnet",
            WorkingDirectory = projectPath,
            RedirectStandardInput = true,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
        };
        startInfo.ArgumentList.Add("user-secrets");
        startInfo.ArgumentList.Add("set");
        startInfo.ArgumentList.Add("--project");
        startInfo.ArgumentList.Add(projectPath);

        var process = Process.Start(startInfo) ?? throw new InvalidOperationException("Failed to start dotnet user-secrets.");
        var json = JsonSerializer.Serialize(values);
        await process.StandardInput.WriteAsync(json);
        process.StandardInput.Close();
        await process.WaitForExitAsync();

        if (process.ExitCode != 0)
        {
            var error = await process.StandardError.ReadToEndAsync();
            throw new InvalidOperationException($"dotnet user-secrets failed: {error}");
        }
    }
}
