namespace MainService.Application.Slices.StreamingSlice.DTOs
{
    public class StreamingDTO
    {
        string TargetUserId { get; set; }
        object Candidate { get; set; }
        object Signal { get; set; }
    }
}
