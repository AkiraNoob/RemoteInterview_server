using MainService.Application.Exceptions;
using MainService.Application.Interfaces;
using MainService.Application.Slices.ReviewSlices.DTOs;
using MainService.Application.Slices.UserSlice.DTOs;
using MainService.Application.Slices.UserSlice.Interfaces;
using MainService.Domain.Models;
using Mapster;
using MediatR;

namespace MainService.Application.Slices.ReviewSlices.Requests;

public class CreateReviewRequest : IRequest<ReviewDTO>
{
    public string Content { get; set; }
    public double Rating { get; set; }
    public Guid CompanyId { get; set; }
}

public class CreateReviewRequestHandler(IRepository<Review> reviewRepository, ICurrentUser currentUser, IUserService userService) : IRequestHandler<CreateReviewRequest, ReviewDTO>
{
    public async Task<ReviewDTO> Handle(CreateReviewRequest request, CancellationToken cancellationToken)
    {
        var _ =await userService.GetUserDetailAsync(request.CompanyId.ToString(), cancellationToken)
            ?? throw new NotFoundException("Company not found.");

        var reviewer = await userService.GetUserDetailAsync(currentUser.GetUserId().ToString(), cancellationToken)
            ?? throw new NotFoundException("Reviewer not found.");

        var review = new Review()
        {
            Content = request.Content,
            Rating = request.Rating,
            CompanyId = request.CompanyId,
            ReviewerId = currentUser.GetUserId()
        };

        await reviewRepository.AddAsync(review, cancellationToken);

        var response = review.Adapt<ReviewDTO>();

        response.Reviewer = reviewer.Adapt<ShortenUserDetailDTO>();
        response.IsOwner = true;

        return response;
    }
}
