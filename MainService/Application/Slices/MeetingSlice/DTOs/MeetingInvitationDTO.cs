using MainService.Domain.Enums;

namespace MainService.Application.Slices.MeetingSlice.DTOs;

public class MeetingInvitationDTO
{
    public Guid Id { get; set; }
    public Guid MeetingId { get; set; }
    public Guid UserId { get; set; }
    public MeetingRoleEnum Role { get; set; }
    public UserMeetingStatusEnum Status { get; set; } = UserMeetingStatusEnum.Pending;
    public string EmployerName { get; set; } = string.Empty;
    public string RecruitmenTitle { get; set; } = string.Empty;
    public DateTime StartTime { get; set; }
    public DateTime? EndTime { get; set; }
}
