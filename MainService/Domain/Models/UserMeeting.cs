using MainService.Domain.Contracts;
using MainService.Domain.Enums;

namespace MainService.Domain.Models;

public class UserMeeting : AuditableEntity, IAggregateRoot
{
    public Guid MeetingId { get; set; }
    public Guid UserId { get; set; }
    public MeetingRoleEnum Role { get; set; }
    public UserMeetingStatusEnum Status { get; set; } = UserMeetingStatusEnum.Pending;
    public virtual Meeting Meeting { get; set; }
}
