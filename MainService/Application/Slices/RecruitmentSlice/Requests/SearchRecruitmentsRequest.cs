using MainService.Application.Common.Models;

namespace MainService.Application.Slices.RecruitmentSlice.Requests;

public class SearchRecruitmentsRequest : PaginationFilter
{
    public ICollection<Guid>? KeywordIds { get; set; }
    public long? MinSalary {  get; set; }
    public long? MaxSalary {  get; set; }
    public int? MinExperience { get; set; }
    public int? ProvinceId { get; set; }
    public int? DistrictId { get; set; }
}
