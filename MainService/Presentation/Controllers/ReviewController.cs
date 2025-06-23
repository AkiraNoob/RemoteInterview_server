using MainService.Application.Common.Models;
using MainService.Application.Slices.ReviewSlices.DTOs;
using MainService.Application.Slices.ReviewSlices.Requests;
using Microsoft.AspNetCore.Mvc;

namespace MainService.Presentation.Controllers;

public class ReviewController : VersionedApiController
{
    [HttpGet("{userId:guid}")]
    [EndpointDescription("Get review of a user")]
    public async Task<PaginationResponse<ReviewDTO>> GetReviewDTOsAsync(Guid userId, CancellationToken cancellationToken)
    {
        return await Mediator.Send(new GetReviewsRequest() { UserId = userId}, cancellationToken);
    }

    [HttpPost("{userId:guid}")]
    [EndpointDescription("Create a review for a user")]
    public async Task<ReviewDTO> CreateReviewAsync(Guid userId, [FromBody] CreateReviewRequest request, CancellationToken cancellationToken)
    {
        return await Mediator.Send(request, cancellationToken);
    }

    [HttpPut("{reviewId:guid}")]
    [EndpointDescription("Update a review for a user")]
    public async Task<ActionResult> UpdateReviewAsync(Guid reviewId, [FromBody] UpdateReviewRequest request, CancellationToken cancellationToken)
    {
        return Ok(await Mediator.Send(request, cancellationToken));
    }

    [HttpDelete("{reviewId:guid}")]
    [EndpointDescription("Delete a review for a user")]
    public async Task<ActionResult> UpdateReviewAsync(Guid reviewId, [FromBody] DeleteReviewRequest request, CancellationToken cancellationToken)
    {
        return Ok(await Mediator.Send(request, cancellationToken));
    }

}
