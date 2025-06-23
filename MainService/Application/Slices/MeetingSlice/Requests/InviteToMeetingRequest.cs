using Ardalis.Result;
using MainService.Application.Exceptions;
using MainService.Application.Interfaces;
using MainService.Application.Slices.MeetingSlice.Specifications;
using MainService.Domain.Models;
using MediatR;

namespace MainService.Application.Slices.MeetingSlice.Requests;

public class InviteToMeetingRequest : IRequest<Result>
{
    public Guid UserId { get; set; }
    public Guid MeetingId { get; set; }
}

public class InviteToMeetingRequestHandler(
    IRepository<UserMeeting> userMeetingRepository,
    IRepository<Meeting> meetingRepository,
    ICurrentUser currentUser
    ) : IRequestHandler<InviteToMeetingRequest, Result>
{

    public async Task<Result> Handle(InviteToMeetingRequest request, CancellationToken cancellationToken)
    {
        var meeting = await meetingRepository.FirstOrDefaultAsync(new GetMeetingByIdSpec(request.MeetingId), cancellationToken) ?? throw new NotFoundException("Meeting not found.");

        if (meeting.OwnerId != currentUser.GetUserId())
        {
            throw new ForbiddenException("Your are not meeting owner.");
        }

        var slot = new UserMeeting()
        {
            UserId = request.UserId,
            MeetingId = request.MeetingId,
            Role = Domain.Enums.MeetingRoleEnum.Guest,
        };

        await userMeetingRepository.AddAsync(slot, cancellationToken);

        return Result.SuccessWithMessage("Invite user successfully.");
    } 
}