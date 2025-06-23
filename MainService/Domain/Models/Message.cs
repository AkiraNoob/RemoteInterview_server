using MainService.Domain.Contracts;

namespace MainService.Domain.Models;

public class Message : AuditableEntity, IAggregateRoot
{
    public string Content { get; set; }
    public Guid SenderId { get; set; }
    public Guid ReceiverId { get; set; }
}
