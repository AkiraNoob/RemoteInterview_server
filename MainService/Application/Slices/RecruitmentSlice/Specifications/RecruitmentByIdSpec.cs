using Ardalis.Specification;
using MainService.Domain.Models;

namespace MainService.Application.Slices.RecruitmentSlice.Specifications;

public class RecruitmentByIdSpec : Specification<Recruitment>
{
    public RecruitmentByIdSpec(Guid recruitmentId)
    {
        Query.Where(u => u.Id == recruitmentId);
    }
}
