using MainService.Domain.Contracts;

namespace MainService.Domain.Models.Streaming;

public class RoomUser : AuditableEntity, IAggregateRoot
{
    public Guid UserId { get; set; }
    public Guid RoomId { get; set; }
    public virtual Room Room { get; set; }

    public RoomUser(Guid userId, Guid roomId)
    {
        UserId = userId;
        RoomId = roomId;
    }
}
