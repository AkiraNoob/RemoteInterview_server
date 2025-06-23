using MainService.Application.Exceptions;
using MainService.Application.Interfaces;
using MainService.Application.Slices.MeetingSlice.DTOs;
using MainService.Application.Slices.MeetingSlice.Specifications;
using MainService.Application.Slices.UserSlice.Interfaces;
using MainService.Domain.Models;
using Mapster;
using MediatR;

namespace MainService.Application.Slices.MeetingSlice.Requests;

public class GetListMeetingInvitationRequest : IRequest<ICollection<MeetingInvitationDTO>>
{
}

public class GetListMeetingInvitationRequestHandler(IRepository<UserMeeting> userMeetingRepository, ICurrentUser currentUser, IUserService userService) : IRequestHandler<GetListMeetingInvitationRequest, ICollection<MeetingInvitationDTO>>
{
    public async Task<ICollection<MeetingInvitationDTO>> Handle(GetListMeetingInvitationRequest request, CancellationToken cancellationToken)
    {
        var invitations = await userMeetingRepository.ListAsync(new GetListMeetingInvitationSpec(request, currentUser.GetUserId()), cancellationToken);
        List<MeetingInvitationDTO> response = new();

        foreach (var invitation in invitations)
        {
            var user = await userService.GetUserDetailAsync(invitation.Meeting!.OwnerId.ToString(), cancellationToken)
                ?? throw new NotFoundException("User not found.");

            var result = invitation.Adapt<MeetingInvitationDTO>();
            result.EmployerName = user.FullName;
            
            response.Add(result);
        }

        return response;
    }
}