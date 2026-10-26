namespace Coffeehouse.Services;

public static class ApiSettings
{
    // The base address with trailing slash required by HttpClient.
    // For Android emulator: 10.0.2.2 points to host loopback.
    // For Windows machine: localhost.
    public static string BaseAddress =>
        DeviceInfo.Platform == DevicePlatform.Android
            ? "http://10.0.2.2:5032/"
            : "http://localhost:5032/";

    public static readonly TimeSpan RequestTimeout = TimeSpan.FromSeconds(15);
}
