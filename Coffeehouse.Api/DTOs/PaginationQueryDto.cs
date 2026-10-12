namespace Coffeehouse.Api.DTOs;

/// <summary>
/// Data transfer object representing query parameters for paginated requests.
/// </summary>
public class PaginationQueryDto
{
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}
