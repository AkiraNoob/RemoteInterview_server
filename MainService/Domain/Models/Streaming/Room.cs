using MainService.Domain.Contracts;

namespace MainService.Domain.Models.Streaming;

public class Room : AuditableEntity, IAggregateRoot
{
    public virtual ICollection<RoomUser> Users { get; set; }
}