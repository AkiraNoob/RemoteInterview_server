namespace MainService.Application.Slices.RecruitmentSlice.DTOs;

public class RecruitmentDTO
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    //public string Motivation { get; set; }
    public string Description { get; set; }
    public string Requirement { get; set; }
    public string Welfare { get; set; }
    public string Address { get; set; }
    public int MinExperience { get; set; }
    public long MaxSalary { get; set; }
    public int ProvinceId { get; set; }
    public int DistrictId { get; set; }
    public string CompanyId { get; set; }
    public string CompanyName { get; set; }
    public string CompanyAddress { get; set; }
    public int? NumberOfApplicant { get; set; } = null;
    public string ProfessionName { get; set; }
    public DateTime TimeStamp { get; set; }
    public DateTime ExpiredDate { get; set; }
}
