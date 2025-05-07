using Ardalis.Specification;
using MainService.Application.Slices.RecruitmentSlice.Requests;
using MainService.Domain.Models;
using TMS.Application.Common.Specification;

namespace MainService.Application.Slices.RecruitmentSlice.Specifications;

public class GetListRecruitmentSpec : EntitiesBySimplePaginationSpec<Recruitment>
{
    public GetListRecruitmentSpec(GetListRecruitmentsRequest request) : base(request)
    {
        if(request.MinSalary != null)
        {
            Query.Where(x => x.MaxSalary >= request.MinSalary);
        }

        if (request.MinExperience != null)
        {
            Query.Where(x => x.MinExperience >= request.MinExperience);
        }

        if (request.ProvinceId != null && request.DistrictId != null)
        {
            Query.Where(x => x.ProvinceId == request.ProvinceId && x.DistrictId == request.DistrictId);
        }

        if (request.TagIds != null && request.TagIds.Count > 0)
        {
            Query.Where(r => r.RecruitmentTags.Any(rt => request.TagIds.Contains(rt.TagId)));
        }
    }
}