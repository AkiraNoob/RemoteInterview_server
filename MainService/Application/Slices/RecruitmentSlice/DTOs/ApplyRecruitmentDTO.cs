namespace MainService.Application.Slices.RecruitmentSlice.DTOs;

public class ApplyRecruitmentDTO
{
    public IFormFile? CV { get; set; }
    public bool UsePreloadedCV { get; set; } = false;
}
