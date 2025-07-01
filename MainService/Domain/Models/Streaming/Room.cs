using MainService.Domain.Contracts;

namespace MainService.Domain.Models.Streaming;

public class Room : AuditableEntity, IAggregateRoot
{
    public Guid MeetingId { get; set; }
    public virtual Meeting Meeting { get; set; }
    public virtual ICollection<RoomUser> Users { get; set; }
}