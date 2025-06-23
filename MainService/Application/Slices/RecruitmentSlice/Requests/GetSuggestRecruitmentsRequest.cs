using MainService.Application.Common.Models;

namespace MainService.Application.Slices.RecruitmentSlice.Requests;

public class GetSuggestRecruitmentsRequest : SimplePaginationFilter
{
    public Guid RecruitmentId { get; set; }
}
