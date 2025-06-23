using MainService.Application.Common.Models;

namespace MainService.Application.Slices.RecruitmentSlice.Requests;

public class GetNewestRecruitmentsRequest : SimplePaginationFilter
{
    public Guid? TagId { get; set; }
    public long? MinSalary { get; set; }
    public int? MinExperience { get; set; }
    public int? ProvinceId { get; set; }
}
