using Ardalis.Specification;
using MainService.Application.Slices.MeetingSlice.Requests;
using MainService.Domain.Models;

namespace MainService.Application.Slices.MeetingSlice.Specifications;

public class GetListMeetingInvitationSpec : Specification<UserMeeting>
{
    public GetListMeetingInvitationSpec(GetListMeetingInvitationRequest request, Guid userId)
    {
        Query
            .Include(x => x.Meeting)
                .ThenInclude(x => x.Recruitment)
            .Where(x => x.UserId == userId && x.Status == Domain.Enums.UserMeetingStatusEnum.Pending && x.Role == Domain.Enums.MeetingRoleEnum.Guest);
    }
}
