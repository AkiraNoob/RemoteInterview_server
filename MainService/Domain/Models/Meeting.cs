using MainService.Domain.Contracts;

namespace MainService.Domain.Models;

public class Meeting : AuditableEntity, IAggregateRoot
{
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public Guid RecruitmentId { get; set; }
    public Guid OwnerId { get; set; }
    public string Description { get; set; }
    public string Title { get; set; }
    public virtual Recruitment Recruitment { get; set; }
    public virtual ICollection<UserMeeting> UserMeetings { get; set; }
    public virtual ICollection<Message> Messages { get; set; }
}