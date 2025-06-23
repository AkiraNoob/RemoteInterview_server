using Ardalis.Specification;
using MainService.Application.Slices.RecruitmentSlice.Requests;
using MainService.Domain.Enums;
using MainService.Domain.Models;
using TMS.Application.Common.Specification;

namespace MainService.Application.Slices.RecruitmentSlice.Specifications;

public class GetRecruitmentOfSpecificEmployerSpec : EntitiesBySimplePaginationSpec<Recruitment>
{
    public GetRecruitmentOfSpecificEmployerSpec(GetRecruitmentOfSpecificEmployerRequest request) : base(request)
    {
        Query
            .Include(x => x.UserRecruitments)
            .Include(x => x.Profession)
            .Where(x => x.CompanyId == request.EmployerId);

        if (request.Status != null)
        {
            Query
                .Where(x => request.Status == RecruitmentStatusEnum.Open ? x.ExpiredDate > DateTime.UtcNow : x.ExpiredDate <= DateTime.UtcNow);
        }
    }
}


public class GetRecruitmentOfSpecificEmployerNoPaginationSpec : Specification<Recruitment>
{
    public GetRecruitmentOfSpecificEmployerNoPaginationSpec(GetRecruitmentOfSpecificEmployerRequest request) 
    {
        Query
            .Where(x => x.CompanyId == request.EmployerId)
            .Where(x => request.Status == RecruitmentStatusEnum.Open ? x.ExpiredDate > DateTime.UtcNow : x.ExpiredDate <= DateTime.UtcNow);
    }
}
