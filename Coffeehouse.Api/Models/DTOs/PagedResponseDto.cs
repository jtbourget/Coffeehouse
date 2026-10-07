namespace Coffeehouse.Api.Models.DTOs;

/// <summary>
/// Data transfer object representing a paginated response collection.
/// </summary>
/// <typeparam name="T">The type of elements contained in the response.</typeparam>
public class PagedResponseDto<T>
{
    public required IReadOnlyCollection<T> Items { get; init; }
    public int TotalCount { get; init; }
    public int PageNumber { get; init; }
    public int PageSize { get; init; }
}
