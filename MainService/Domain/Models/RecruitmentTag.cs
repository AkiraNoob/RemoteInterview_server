using MainService.Domain.Contracts;

namespace MainService.Domain.Models;

public class RecruitmentTag : AuditableEntity, IAggregateRoot
{
    public Guid TagId { get; set; }
    public Guid RecruitmentId { get; set; }
    public virtual Recruitment Recruitment { get; set; }
    public virtual Tag Tag { get; set; }
}
