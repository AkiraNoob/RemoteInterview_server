namespace MainService.Application.Slices.StreamingSlice.DTOs
{
    public class IceCandidateDTO
    {
        string TargetUserId { get; set; }
        object Candidate { get; set; }
        string SenderUserId { get; set; }
    }
}
