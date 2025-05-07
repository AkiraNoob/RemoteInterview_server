using Ardalis.Specification;
using MainService.Domain.Models;

namespace MainService.Application.Slices.RecruitmentSlice.Specifications;

public class GetRecruitmentByIdSpec : Specification<Recruitment>
{
    public GetRecruitmentByIdSpec(Guid recruitmentId)
    {
        Query.Where(u => u.Id == recruitmentId);
    }
}
