using Ardalis.Specification;
using MainService.Domain.Models;

namespace MainService.Application.Slices.RecruitmentSlice.Specifications;

public class UserRecuitmentByRecruimentIdSpec : Specification<UserRecruitment>
{
    public UserRecuitmentByRecruimentIdSpec(string recruitmentId)
    {
        Query
            .Include(u => u.AppliedCv)
            .Where(u => u.RecruitmentId.ToString() == recruitmentId);
    }
}
