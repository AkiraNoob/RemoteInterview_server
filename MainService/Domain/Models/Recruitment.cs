using MainService.Domain.Contracts;

namespace MainService.Domain.Models;

public class Recruitment(Guid companyId,
    string title,
    string motivation,
    string description,
    string requirement,
    string welfare,
    string address,
    int provinceId,
    int districtId,
    int minExperience,
    long maxSalary) : AuditableEntity, IAggregateRoot
{
    public Guid CompanyId { get; set; } = companyId;
    public string Title { get; set; } = title;
    public string Motivation { get; set; } = motivation;
    public string Description { get; set; } = description;
    public string Requirement { get; set; } = requirement;
    public string Welfare { get; set; } = welfare;
    public string Address { get; set; } = address;
    public int ProvinceId { get; set; } = provinceId;
    public int DistrictId { get; set; } = districtId;
    public int MinExperience { get; set; } = minExperience;
    public long MaxSalary { get; set; } = maxSalary;

    public virtual ICollection<Meeting> Meetings { get; set; }
    public virtual ICollection<UserRecruitment> UserRecruitments { get; set; }
    public virtual ICollection<RecruitmentTag> RecruitmentTags { get; set; }
}
