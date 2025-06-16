namespace MainService.Application.Slices.StreamingSlice.DTOs;

public class OfferStreamingDTO : ForwardStreamingMessageBaseDTO
{
    public object Signal { get; set; }
}

public class RoutingOfferReceivedStreamingDTO
{
    public object Signal { get; set; }
    public string SourceConnectionId { get; set; }
    public RoutingOfferReceivedStreamingDTO(object Signal, string SourceConnectionId)
    {
        this.Signal = Signal;
        this.SourceConnectionId = SourceConnectionId;
    }
    public RoutingOfferReceivedStreamingDTO(OfferStreamingDTO offer)
    {
        this.Signal = offer.Signal;
        this.SourceConnectionId = offer.CallerConnectionId;
    }
}
