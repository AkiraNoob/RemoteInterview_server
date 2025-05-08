using MainService.Domain.Contracts;

namespace MainService.Domain.Models;

public class Tag : AuditableEntity, IAggregateRoot
{
    public string Name {  get; set; }
}
