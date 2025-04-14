namespace MainService.Application.Common.Models;

public class SimplePaginationFilter
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = int.MaxValue;
}