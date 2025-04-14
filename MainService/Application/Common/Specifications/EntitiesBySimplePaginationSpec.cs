using Ardalis.Specification;
using MainService.Application.Common.Models;

namespace TMS.Application.Common.Specification;

public class EntitiesBySimplePaginationSpec<T, TResult> : Specification<T, TResult>
{
    public EntitiesBySimplePaginationSpec(SimplePaginationFilter filter)
    {
        Query
            .Skip((filter.PageNumber - 1) * filter.PageSize)
            .Take(filter.PageSize);
    }
}

public class EntitiesBySimplePaginationSpec<T> : Specification<T>
{
    public EntitiesBySimplePaginationSpec(SimplePaginationFilter filter)
    {
        Query
            .Skip((filter.PageNumber - 1) * filter.PageSize)
            .Take(filter.PageSize);
    }
}