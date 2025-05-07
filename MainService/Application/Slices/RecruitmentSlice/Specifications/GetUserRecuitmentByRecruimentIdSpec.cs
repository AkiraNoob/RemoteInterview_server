using Ardalis.Specification;
using MainService.Domain.Models;

namespace MainService.Application.Slices.RecruitmentSlice.Specifications;

public class GetUserRecuitmentByRecruimentIdSpec : Specification<UserRecruitment>
{
    public GetUserRecuitmentByRecruimentIdSpec(string recruitmentId)
    {
        Query
            .Include(u => u.AppliedCv)
            .Where(u => u.RecruitmentId.ToString() == recruitmentId);
    }
}
