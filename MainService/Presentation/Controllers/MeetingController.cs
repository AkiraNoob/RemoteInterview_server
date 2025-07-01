using MainService.Application.Slices.MeetingSlice.DTOs;
using MainService.Application.Slices.MeetingSlice.Requests;
using MainService.Domain.Models.Streaming;
using MainService.Infrastructure.Persistence.Context;
using Microsoft.AspNetCore.Mvc;

namespace MainService.Presentation.Controllers;

public class MeetingController(ApplicationDbContext applicationDbContext) : VersionedApiController
{
    [HttpGet("list")]
    [EndpointDescription("Get all meeting schedule.")]
    public async Task<ICollection<ShortenMeetingDTO>> GetListMeetingAsync([FromQuery] GetListMeetingRequest body, CancellationToken cancellationToken)
    {
        return await Mediator.Send(body, cancellationToken);
    }

    [HttpPost("")]
    [EndpointDescription("Create a meeting schedule.")]
    public async Task<ShortenMeetingDTO> CreateMeetingAsync([FromBody] CreateMeetingRequest body, CancellationToken cancellationToken)
    {
        return await Mediator.Send(body, cancellationToken);
    }

    [HttpPut("")]
    [EndpointDescription("Update a meeting schedule.")]
    public async Task<ShortenMeetingDTO> UpdateMeetingAsync([FromBody] UpdateMeetingRequest body, CancellationToken cancellationToken)
    {
        return await Mediator.Send(body, cancellationToken);
    }

    [HttpGet("{meetingId:guid}")]
    [EndpointDescription("Get meeting detail by Id.")]
    public async Task<DetailedMeetingDTO> GetMeetingByIdAsync(Guid meetingId, CancellationToken cancellationToken)
    {
        return await Mediator.Send(new GetMeetingDetailRequest() {
        MeetingId = meetingId
        }, cancellationToken);
    }

    [HttpPost("invite")]
    [EndpointDescription("Invite user to meeting.")]
    public async Task<IActionResult> InviteUserToMeetingAsync([FromBody] InviteToMeetingRequest body, CancellationToken cancellationToken)
    {
        await Mediator.Send(body, cancellationToken);
        return Ok();
    }

    [HttpPut("invite/{invitationId:guid}")]
    [EndpointDescription("Respond meeting invitation.")]
    public async Task<IActionResult> ResponseMeetingInvitationAsync(Guid invitationId, [FromBody] RespondMeetingInvitationDTO body, CancellationToken cancellationToken)
    {
        await Mediator.Send(new RespondMeetingInvitationRequest(invitationId, body), cancellationToken);
        return Ok();
    }

    [HttpGet("invitations")]
    [EndpointDescription("Get all pending meeting invitation.")]
    public async Task<ICollection<MeetingInvitationDTO>> GetAllPendingMeetingInvitation(CancellationToken cancellationToken)
    {
        return await Mediator.Send(new GetListMeetingInvitationRequest(), cancellationToken);
    }

    //[HttpPost("{meetingId:guid}/start")]
    //[EndpointDescription("Get all pending meeting invitation.")]
    //public async Task<ICollection<MeetingInvitationDTO>> StartMeetingAysnc(CancellationToken cancellationToken)
    //{
    //}
}
