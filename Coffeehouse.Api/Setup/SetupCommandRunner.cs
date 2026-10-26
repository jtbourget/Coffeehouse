namespace Coffeehouse.Api.Setup;

public static class SetupCommandRunner
{
    public static bool IsCommandMode(string[] args, out string command)
    {
        command = string.Empty;
        if (args.Length == 0) return false;

        if (args[0].StartsWith("--"))
        {
            command = args[0][2..];
            return true;
        }

        return false;
    }
}
