using MainService.Domain.Contracts;

namespace MainService.Domain.Models;

public class Message : AuditableEntity, IAggregateRoot
{
    public string Content { get; set; }
    public Guid SenderId { get; set; }
    public Guid MeetingId { get; set; }
    public virtual Meeting Meeting { get; set; }
}
