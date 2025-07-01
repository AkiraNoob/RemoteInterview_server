using MainService.Application.Slices.FileSlice.DTOs;
using MainService.Application.Slices.UserSlice.DTOs;
using MainService.Domain.Enums;

namespace MainService.Application.Slices.RecruitmentSlice.DTOs;

public class RecruitmentApplyingResultDTO
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid RecruitmentId { get; set; }
    public FileDTO AppliedCv { get; set; }
    public ShortenUserDetailDTO User { get; set; }
    public UserRecruitmentStatusEnum Status { get; set; }
}
