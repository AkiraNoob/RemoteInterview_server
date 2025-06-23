using MainService.Domain.Contracts;

namespace MainService.Domain.Models;

public class RecruitmentKeyword : AuditableEntity, IAggregateRoot
{
    public Guid KeywordId { get; set; }
    public Guid RecruitmentId { get; set; }
    public virtual Recruitment Recruitment { get; set; }
    public virtual Keyword Keyword { get; set; }
}
