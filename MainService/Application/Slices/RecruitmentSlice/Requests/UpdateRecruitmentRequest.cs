using MainService.Application.Slices.RecruitmentSlice.DTOs;
using Mapster;

namespace MainService.Application.Slices.RecruitmentSlice.Requests;

public class UpdateRecruitmentRequest : UpdateRecruitmentDTO
{
    public Guid RecruitmentId { get; set; }

    public UpdateRecruitmentRequest(Guid recruitmentId, UpdateRecruitmentDTO dto)
    {
        RecruitmentId = recruitmentId;
        dto.Adapt(this);
    }
}

