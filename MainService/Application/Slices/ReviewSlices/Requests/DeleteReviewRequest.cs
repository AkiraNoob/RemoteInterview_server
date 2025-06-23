using Ardalis.Result;
using MainService.Application.Exceptions;
using MainService.Application.Interfaces;
using MainService.Domain.Models;
using MediatR;

namespace MainService.Application.Slices.ReviewSlices.Requests;

public class DeleteReviewRequest : IRequest<Result>
{
    public Guid ReviewId { get; set; }
}

public class DeleteReviewRequestHandler(IRepository<Review> reviewRepository, ICurrentUser currentUser) : IRequestHandler<DeleteReviewRequest, Result>
{
    public async Task<Result> Handle(DeleteReviewRequest request, CancellationToken cancellationToken)
    {
        var review = await reviewRepository.GetByIdAsync(request.ReviewId, cancellationToken)
            ?? throw new NotFoundException("Review not found.");

        if (review.ReviewerId != currentUser.GetUserId())
            throw new ForbiddenException("You are not authorized to delete this review.");

        await reviewRepository.DeleteAsync(review, cancellationToken);

        return Result.SuccessWithMessage("Review deleted successfully.");
    }
}
