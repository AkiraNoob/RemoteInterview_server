using MainService.Domain.Contracts;

namespace MainService.Domain.Models;

public class Profession : AuditableEntity, IAggregateRoot
{
    public string Name { get; set; }
    public Profession(string name)
    {
        Name = name;
    }
}
