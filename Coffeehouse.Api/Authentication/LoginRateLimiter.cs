using Microsoft.AspNetCore.Http.Features;

namespace Coffeehouse.Api.Authentication;

public class LoginRateLimiter
{
    private readonly RateLimitOptions _options;
    private readonly Dictionary<string, List<DateTime>> _attempts = new();

    public LoginRateLimiter(RateLimitOptions options)
    {
        _options = options;
    }

    public bool IsAllowed(IFeatureCollection features)
    {
        var connection = features.Get<IHttpConnectionFeature>();
        var key = connection?.RemoteIpAddress?.ToString() ?? "anonymous";
        var now = DateTime.UtcNow;
        var cutoff = now - _options.Window;

        lock (_attempts)
        {
            if (_attempts.TryGetValue(key, out var list))
            {
                list.RemoveAll(t => t < cutoff);
                if (list.Count >= _options.MaxAttempts) return false;
                list.Add(now);
            }
            else
            {
                _attempts[key] = [now];
            }
        }

        return true;
    }
}
