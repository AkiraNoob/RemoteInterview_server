using MainService.Application.Common.Models;
using MainService.Application.Interfaces;
using MainService.Application.Slices.RecruitmentSlice.DTOs;
using MainService.Application.Slices.RecruitmentSlice.Interfaces;
using MainService.Application.Slices.RecruitmentSlice.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MainService.Presentation.Controllers;

public class RecruitmentController(IRecruitmentService recruitmentService) : VersionedApiController
{
    [HttpPost("")]
    [EndpointDescription("Create a recruitment")]
    public async Task<RecruitmentDTO> CreateRecruitmentAsync([FromBody] CreateRecruitmentRequest request, CancellationToken cancellationToken)
    {
        return await recruitmentService.CreateRecruitmentAsync(request, cancellationToken);
    }

    [HttpPut("{recruitmentId:guid}")]
    [EndpointDescription("Update a recruitment")]
    public async Task<RecruitmentDTO> CreateRecruitmentAsync(Guid recruitmentId, [FromBody] UpdateRecruitmentDTO request, CancellationToken cancellationToken)
    {
        return await recruitmentService.UpdateRecruitmentAsync(new UpdateRecruitmentRequest(recruitmentId, request), cancellationToken);
    }

    [HttpPut("apply/{recruitmentId:guid}")]
    [EndpointDescription("Apply for a recruitment")]
    public async Task<Guid> ApplyRecruitmentAsync(Guid recruitmentId, [FromForm] ApplyRecruitmentDTO request, CancellationToken cancellationToken)
    {
        return await Mediator.Send(new ApplyRecruimentRequest(recruitmentId, request.CV, request.UsePreloadedCV), cancellationToken);
    }

    [AllowAnonymous]
    [HttpGet("list")]
    [EndpointDescription("Get list recruitment using filter")]
    public async Task<PaginationResponse<RecruitmentDTO>> GetRecruitmentsAsync([FromQuery] SearchRecruitmentsRequest request, CancellationToken cancellationToken)
    {
        return await recruitmentService.GetListRecruitmentsAsync(request, cancellationToken);
    }

    [AllowAnonymous]
    [HttpGet("newest")]
    [EndpointDescription("Get list newest recruitment")]
    public async Task<PaginationResponse<RecruitmentDTO>> GetNewestRecruitmentsAsync([FromQuery] GetNewestRecruitmentsRequest request, CancellationToken cancellationToken)
    {
        return await recruitmentService.GetNewestRecruitmentsAsync(request, cancellationToken);
    }

    [AllowAnonymous]
    [HttpGet("{recruitmentId:guid}")]
    [EndpointDescription("Get recruitment detail")]
    public async Task<RecruitmentDTO> GetRecruitmentDetailAsync(Guid recruitmentId, CancellationToken cancellationToken)
    {
        return await recruitmentService.GetRecruitmentByIdAsync(recruitmentId, cancellationToken);
    }

    [HttpGet("{recruitmentId:guid}/apply")]
    [EndpointDescription("Get applying list of a recruitment")]
    public async Task<PaginationResponse<RecruitmentApplyingResultDTO>> GetApplyingListOfARecruitmentAsync([FromQuery] GetApplyingListOfRecruitmentRequest request, CancellationToken cancellationToken)
    {
        return await Mediator.Send(request, cancellationToken);
    }

    [HttpGet("list/{userId:guid}/")]
    [EndpointDescription("Get list recruitments of a specific user")]
    public async Task<PaginationResponse<RecruitmentDTO>> GetRecruitmentOfSpecificEmployerAsync([FromQuery] GetRecruitmentOfSpecificEmployerRequest request, CancellationToken cancellationToken)
    {
        return await Mediator.Send(request, cancellationToken);
    }


    [HttpGet("suggest")]
    [EndpointDescription("Get list recruitments of a specific user")]
    public async Task<PaginationResponse<RecruitmentDTO>> GetSuggestRecruitments([FromQuery] GetSuggestRecruitmentsRequest request, CancellationToken cancellationToken)
    {
        return await recruitmentService.GetSuggestRecruitmentsAsync(request, cancellationToken);
    }

    [HttpPut("respond/{applicationId:guid}")]
    [EndpointDescription("Respond to recruitment applying")]
    public async Task<string> RespondToRecruitmentApplyingAsync(Guid applicationId, [FromBody] RespondRecruitmentApplicationDTO request, CancellationToken cancellationToken)
    {
        return await Mediator.Send(new RespondRecruitmentApplicationRequest(applicationId, request), cancellationToken);
    }
}
