using Ardalis.Specification;
using MainService.Application.Slices.ReviewSlices.Requests;
using MainService.Domain.Models;
using TMS.Application.Common.Specification;

namespace MainService.Application.Slices.ReviewSlices.Specification
{
    public class GetReviewsSpec : EntitiesBySimplePaginationSpec<Review>
    {
        public GetReviewsSpec(GetReviewsRequest request) : base(request)
        {
            Query.Where(x => x.CompanyId == request.UserId);
        }
    }

    public class GetReviewsNoPaginationSpec : Specification<Review>
    {
        public GetReviewsNoPaginationSpec(GetReviewsRequest request) 
        {
            Query.Where(x => x.CompanyId == request.UserId);
        }
    }
}
