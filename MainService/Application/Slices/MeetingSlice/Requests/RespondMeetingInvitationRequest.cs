using Ardalis.Result;
using MainService.Application.Exceptions;
using MainService.Application.Interfaces;
using MainService.Application.Slices.MeetingSlice.DTOs;
using MainService.Domain.Enums;
using MainService.Domain.Models;
using MediatR;

namespace MainService.Application.Slices.MeetingSlice.Requests;

public class RespondMeetingInvitationRequest : RespondMeetingInvitationDTO, IRequest<Result>
{
    public Guid InvitationId { get; set; }

    public RespondMeetingInvitationRequest(Guid invitationId, RespondMeetingInvitationDTO dto)
    {
        Status = dto.Status;
        InvitationId = invitationId;
    }
}

public class ResponseMeetingInvitationRequestHandler(IRepository<UserMeeting> userMeetingRepository) : IRequestHandler<RespondMeetingInvitationRequest, Result>
{
    public async Task<Result> Handle(RespondMeetingInvitationRequest request, CancellationToken cancellationToken)
    {
        var invitation = await userMeetingRepository.GetByIdAsync(request.InvitationId, cancellationToken)
            ?? throw new NotFoundException("Invitation not found.");

        if (invitation.Status == UserMeetingStatusEnum.Pending)
        {
            invitation.Status = request.Status;
            return Result.SuccessWithMessage("Update invitaion status successfully");
        }

        throw new BadRequestException("Invitation is already accepted or rejected.");
    }
}