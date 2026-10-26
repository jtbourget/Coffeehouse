namespace Coffeehouse.Api.Authentication;

public class RateLimitOptions
{
    public int MaxAttempts { get; set; } = 5;
    public TimeSpan Window { get; set; } = TimeSpan.FromMinutes(1);
}
