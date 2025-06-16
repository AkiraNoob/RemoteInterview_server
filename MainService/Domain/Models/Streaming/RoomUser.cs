using MainService.Domain.Contracts;

namespace MainService.Domain.Models.Streaming;

public class RoomUser(Guid userId, Guid roomId, string connectionId) : AuditableEntity, IAggregateRoot
{
    public Guid UserId { get; set; } = userId;
    public Guid RoomId { get; set; } = roomId;
    public string ConnectionId { get; set; } = connectionId;
    public virtual Room Room { get; set; }
}
