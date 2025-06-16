namespace MainService.Application.Slices.StreamingSlice.DTOs;

public class IceCandidateDTO : ForwardStreamingMessageBaseDTO
{
    public object Candidate { get; set; }
}

public class RoutingIceCandidateReceivedStreamingDTO
{
    public object Candidate { get; set; }
    public string SourceConnectionId { get; set; }
    public RoutingIceCandidateReceivedStreamingDTO(object Candidate, string SourceConnectionId)
    {
        this.Candidate = Candidate;
        this.SourceConnectionId = SourceConnectionId;
    }
    public RoutingIceCandidateReceivedStreamingDTO(IceCandidateDTO ic)
    {
        this.Candidate = ic.Candidate;
        this.SourceConnectionId = ic.ReceiverConnectionId;
    }
}
