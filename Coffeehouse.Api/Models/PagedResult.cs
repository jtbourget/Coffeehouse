namespace Coffeehouse.Api.Models;

/// <summary>
/// Represents a paginated result containing a subset of items and pagination metadata.
/// </summary>
/// <typeparam name="T">The type of elements contained in the result.</typeparam>
public class PagedResult<T>
{
    public required IReadOnlyCollection<T> Items { get; init; }
    public int TotalCount { get; init; }
    public int PageNumber { get; init; }
    public int PageSize { get; init; }
}
