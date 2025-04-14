using MainService.Application.Slices.FileSlice.DTOs;
using MainService.Application.Slices.UserSlice.DTOs;

namespace MainService.Application.Slices.RecruitmentSlice.DTOs;

public class RecruitmentApplyingResultDTO
{
    public Guid UserId { get; set; }
    public Guid RecruitmentId { get; set; }
    public FileDTO CV { get; set; }
    public ShortenUserDetailDTO User { get; set; }
}
