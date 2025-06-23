using MainService.Domain.Contracts;

namespace MainService.Domain.Models;

public class Review : AuditableEntity, IAggregateRoot
{
    public Guid CompanyId { get; set; }
    public double Rating { get; set; }
    public string Content { get; set; }
    public Guid ReviewerId { get; set; }
}
