using Ardalis.Specification;
using MainService.Application.Slices.RecruitmentSlice.Requests;
using MainService.Domain.Models;

namespace MainService.Application.Slices.RecruitmentSlice.Specifications;

public class GetUserRecuitmentByRecruimentIdSpec : Specification<UserRecruitment>
{
    public GetUserRecuitmentByRecruimentIdSpec(GetApplyingListOfRecruitmentRequest request)
    {
        Query
            .Include(u => u.AppliedCv)
            .Include(u => u.Recruitment)
            .Where(u => u.RecruitmentId.ToString() == request.RecruitmentId.ToString());

        if(request.Status != null)
        {
            Query.Where(u => u.Status == request.Status);
        }
    }
}
