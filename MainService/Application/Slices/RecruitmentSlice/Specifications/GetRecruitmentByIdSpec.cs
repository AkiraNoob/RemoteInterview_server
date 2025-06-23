using Ardalis.Specification;
using MainService.Domain.Models;

namespace MainService.Application.Slices.RecruitmentSlice.Specifications;

public class GetRecruitmentByIdSpec : Specification<Recruitment>
{
    public GetRecruitmentByIdSpec(Guid recruitmentId)
    {
        Query
.Include(x => x.UserRecruitments)
.Include(x => x.Profession);

        Query.Where(u => u.Id == recruitmentId);
    }
}
