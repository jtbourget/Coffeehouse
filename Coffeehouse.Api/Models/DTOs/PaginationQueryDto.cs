namespace Coffeehouse.Api.Models.DTOs;

/// <summary>
/// Data transfer object containing pagination parameters for query requests.
/// </summary>
public class PaginationQueryDto
{
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}
