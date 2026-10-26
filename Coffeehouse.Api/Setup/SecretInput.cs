namespace Coffeehouse.Api.Setup;

public static class SecretInput
{
    public static string ReadPassword(string prompt)
    {
        Console.Write(prompt);
        var password = string.Empty;
        while (true)
        {
            var key = Console.ReadKey(intercept: true);
            if (key.Key == ConsoleKey.Enter) break;
            if (key.Key == ConsoleKey.Backspace && password.Length > 0)
            {
                password = password[..^1];
                Console.Write("\b \b");
            }
            else if (key.Key != ConsoleKey.Backspace)
            {
                password += key.KeyChar;
                Console.Write('*');
            }
        }

        Console.WriteLine();
        return password;
    }
}
