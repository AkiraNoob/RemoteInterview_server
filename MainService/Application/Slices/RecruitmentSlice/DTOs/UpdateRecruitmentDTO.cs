namespace MainService.Application.Slices.RecruitmentSlice.DTOs;

public class UpdateRecruitmentDTO
{
    public string? Title { get; set; }
    public string? Motivation { get; set; }
    public string? Description { get; set; }
    public string? Requirement { get; set; }
    public string? Welfare { get; set; }
    public string? Address { get; set; }
    public int? MinExperience { get; set; }
    public long? Salary { get; set; }
    public int? ProvinceId { get; set; }
    public int? DistrictId { get; set; }
}
