using MainService.Domain.Enums;

namespace MainService.Application.Slices.MeetingSlice.DTOs;

public class RespondMeetingInvitationDTO
{
    public UserMeetingStatusEnum Status { get; set; }
}
