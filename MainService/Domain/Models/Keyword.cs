using MainService.Domain.Contracts;

namespace MainService.Domain.Models;

public class Keyword : AuditableEntity, IAggregateRoot
{
    public string Name {  get; set; }
}
