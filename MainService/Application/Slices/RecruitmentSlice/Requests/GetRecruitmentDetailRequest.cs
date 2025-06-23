namespace MainService.Application.Slices.RecruitmentSlice.Requests;

public class GetRecruitmentDetailRequest
{
    public Guid RecruitmentId { get; set; }

    public GetRecruitmentDetailRequest(Guid recruitmentId)
    {
        RecruitmentId = recruitmentId;
    }
}
