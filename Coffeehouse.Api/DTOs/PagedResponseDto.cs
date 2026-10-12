namespace Coffeehouse.Api.DTOs;

/// <summary>
/// Generic data transfer object representing a paginated set of items.
/// </summary>
/// <typeparam name="T">The type of the item in the paginated list.</typeparam>
public class PagedResponseDto<T>
{
    public IEnumerable<T> Items { get; set; } = Enumerable.Empty<T>();
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalCount { get; set; }
    public int TotalPages { get; set; }
}
