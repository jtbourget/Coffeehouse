namespace Coffeehouse.Api.Models.DTOs;

public class PaginationQueryDto
{
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}
