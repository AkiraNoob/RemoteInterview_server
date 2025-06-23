using Ardalis.Specification;
using MainService.Application.Slices.RecruitmentSlice.Requests;
using MainService.Domain.Models;
using System.Linq;
using TMS.Application.Common.Specification;

namespace MainService.Application.Slices.RecruitmentSlice.Specifications;

public class GetNewestRecruitmentsSpec : EntitiesBySimplePaginationSpec<Recruitment>
{
    public GetNewestRecruitmentsSpec(GetNewestRecruitmentsRequest request) : base(request)
    {

        Query
    .Include(x => x.UserRecruitments)
    .Include(x => x.Profession);
        if (request.MinSalary != null)
        {
            Query.Where(x => x.MaxSalary >= request.MinSalary);
        }

        if (request.MinExperience != null)
        {
            Query.Where(x => x.MinExperience >= request.MinExperience);
        }

        if (request.ProvinceId != null)
        {
            Query.Where(x => x.ProvinceId == request.ProvinceId);
        }

        if (request.TagId != null)
        {
            Query.Where(r => r.RecruitmentKeywords.Any(rt => request.TagId == rt.KeywordId));
        }
    }
}

public class GetNewestRecruitmentsNoPaginationSpec : Specification<Recruitment>
{
    public GetNewestRecruitmentsNoPaginationSpec(GetNewestRecruitmentsRequest request)
    {
        if (request.MinSalary != null)
        {
            Query.Where(x => x.MaxSalary >= request.MinSalary);
        }

        if (request.MinExperience != null)
        {
            Query.Where(x => x.MinExperience >= request.MinExperience);
        }

        if (request.ProvinceId != null)
        {
            Query.Where(x => x.ProvinceId == request.ProvinceId);
        }

        if (request.TagId != null)
        {
            Query.Where(r => r.RecruitmentKeywords.Any(rt => request.TagId == rt.KeywordId));
        }
    }
}