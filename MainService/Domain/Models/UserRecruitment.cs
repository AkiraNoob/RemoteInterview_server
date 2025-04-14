using MainService.Domain.Contracts;

namespace MainService.Domain.Models;

public class UserRecruitment : AuditableEntity, IAggregateRoot
{
    public Guid RecruitmentId { get; set; }
    public Guid UserId { get; set; }
    public Guid FileId { get; set; }
    public virtual Recruitment Recruitment { get; set; }
    public virtual File AppliedCv { get; set; }

    public UserRecruitment(Guid userId, Guid recruitmentId)
    {
        UserId = userId;
        RecruitmentId = recruitmentId;
    }
}
