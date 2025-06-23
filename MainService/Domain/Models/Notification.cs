using MainService.Domain.Contracts;
using MainService.Domain.Enums;

namespace MainService.Domain.Models;

public class Notification : AuditableEntity, IAggregateRoot
{
    public Guid UserId { get; set; }
    public NotificationTypeEnum Type { get; set; }
    public Guid ResourceId { get; set; }
}
