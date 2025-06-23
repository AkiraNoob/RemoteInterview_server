using MainService.Application.Common.Models;
using MainService.Application.Exceptions;
using MainService.Application.Interfaces;
using MainService.Application.Slices.ReviewSlices.DTOs;
using MainService.Application.Slices.ReviewSlices.Specification;
using MainService.Application.Slices.UserSlice.Interfaces;
using MainService.Domain.Models;
using Mapster;
using MediatR;

namespace MainService.Application.Slices.ReviewSlices.Requests;

public class GetReviewsRequest : SimplePaginationFilter, IRequest<PaginationResponse<ReviewDTO>>
{
    public Guid UserId { get; set; }
}

public class GetReviewsRequestHandler(IRepository<Review> reviewRepository, IUserService userService, ICurrentUser currentUser) : IRequestHandler<GetReviewsRequest, PaginationResponse<ReviewDTO>>
{
    public async Task<PaginationResponse<ReviewDTO>> Handle(GetReviewsRequest request, CancellationToken cancellationToken)
    {
        var _ = await userService.GetUserDetailAsync(request.UserId.ToString(), cancellationToken) ?? throw new NotFoundException("User not found.");

        var reviews = await reviewRepository.ListAsync(new GetReviewsSpec(request), cancellationToken);

        List<ReviewDTO> result = new ();

        foreach (var item in reviews)
        {
            var reviewer = await userService.GetUserDetailAsync(item.ReviewerId.ToString());
            var dto = new ReviewDTO();

            item.Adapt(dto);

            reviewer?.Adapt(dto.Reviewer);
            dto.IsOwner = currentUser.GetUserId().ToString() == reviewer.Id;

            result.Add(dto);
        }


        var count = await reviewRepository.CountAsync(new GetReviewsNoPaginationSpec(request), cancellationToken);

        return new PaginationResponse<ReviewDTO>(result, count, request.PageNumber, request.PageSize);
    }
}
