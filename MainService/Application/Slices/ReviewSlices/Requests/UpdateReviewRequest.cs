using MainService.Application.Exceptions;
using MainService.Application.Interfaces;
using MainService.Application.Slices.ReviewSlices.DTOs;
using MainService.Domain.Models;
using Mapster;
using MediatR;

namespace MainService.Application.Slices.ReviewSlices.Requests;

public class UpdateReviewRequest : UpdateReviewDTO, IRequest<string>
{
    public Guid ReviewId { get; set; }
    public UpdateReviewRequest(Guid reviewId, UpdateReviewDTO dto)
    {
        ReviewId = reviewId;
        Content = dto.Content;
        Rating = dto.Rating;
    }
}

public class UpdateReviewRequestHandler(IRepository<Review> reviewRepository, ICurrentUser currentUser) : IRequestHandler<UpdateReviewRequest, string>
{
    public async Task<string> Handle(UpdateReviewRequest request, CancellationToken cancellationToken)
    {
        var review = await reviewRepository.GetByIdAsync(request.ReviewId, cancellationToken)
            ?? throw new NotFoundException("Review not found.");

        if (review.ReviewerId != currentUser.GetUserId())
            throw new ForbiddenException("You are not authorized to update this review.");

        request.Adapt(review);

        await reviewRepository.UpdateAsync(review, cancellationToken);

        return review.Id.ToString();
    }
}



