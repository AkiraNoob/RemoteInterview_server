namespace MainService.Application.Slices.StreamingSlice.DTOs;

public class AnswerStreamingDTO : ForwardStreamingMessageBaseDTO
{
    public object Signal { get; set; }
}

public class RoutingAnswerReceivedStreamingDTO
{
    public object Signal { get; set; }
    public string SourceConnectionId { get; set; }
    public RoutingAnswerReceivedStreamingDTO(object Signal, string SourceConnectionId)
    {
        this.Signal = Signal;
        this.SourceConnectionId = SourceConnectionId;
    }
    public RoutingAnswerReceivedStreamingDTO(AnswerStreamingDTO answer)
    {
        this.Signal = answer.Signal;
        this.SourceConnectionId = answer.CallerConnectionId;
    }
}