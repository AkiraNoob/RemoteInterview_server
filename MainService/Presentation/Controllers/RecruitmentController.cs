using MainService.Application.Common.Models;
using MainService.Application.Interfaces;
using MainService.Application.Slices.RecruitmentSlice.DTOs;
using MainService.Application.Slices.RecruitmentSlice.Requests;
using Microsoft.AspNetCore.Mvc;

namespace MainService.Presentation.Controllers;

public class RecruitmentController : VersionedApiController
{
    [HttpPost("")]
    [EndpointDescription("Create a recruitment")]
    public async Task<RecruitmentDTO> CreateRecruitmentAsync([FromBody] CreateRecruitmentRequest request, CancellationToken cancellationToken)
    {
        return await Mediator.Send(request, cancellationToken);
    }

    [HttpPut("{recruitmentId:guid}")]
    [EndpointDescription("Update a recruitment")]
    public async Task<RecruitmentDTO> CreateRecruitmentAsync(Guid recruitmentId, [FromBody] UpdateRecruitmentDTO request, CancellationToken cancellationToken)
    {
        return await Mediator.Send(new UpdateRecruitmentRequest(recruitmentId, request), cancellationToken);
    }

    [HttpPut("apply/{recruitmentId:guid}")]
    [EndpointDescription("Apply for a recruitment")]
    public async Task<Guid> ApplyRecruitmentAsync(Guid recruitmentId, [FromBody] ApplyRecruitmentDTO request, CancellationToken cancellationToken)
    {
        return await Mediator.Send(new ApplyRecruimentRequest(recruitmentId, request.CV, request.UsePreloadedCV), cancellationToken);
    }

    [HttpGet("")]
    [EndpointDescription("Get list recruitment")]
    public async Task<PaginationResponse<RecruitmentDTO>> GetRecruitmentsAsync([FromBody] GetListRecruitmentsRequest request, CancellationToken cancellationToken)
    {
        return await Mediator.Send(request, cancellationToken);
    }

    [HttpGet("{recruitmentId}")]
    [EndpointDescription("Get recruitment detail")]
    public async Task<RecruitmentDTO> GetRecruitmentDetailAsync(Guid recruitmentId, CancellationToken cancellationToken)
    {
        return await Mediator.Send(new GetRecruitmentDetailRequest(recruitmentId), cancellationToken);
    }

    [HttpGet("{recruitmentId}/apply")]
    [EndpointDescription("Get applying list of a recruitment")]
    public async Task<PaginationResponse<RecruitmentApplyingResultDTO>> GetApplyingListOfARecruitmentAsync(Guid recruitmentId, CancellationToken cancellationToken)
    {
        return await Mediator.Send(new GetApplyingListOfRecruitmentRequest(recruitmentId), cancellationToken);
    }
}
