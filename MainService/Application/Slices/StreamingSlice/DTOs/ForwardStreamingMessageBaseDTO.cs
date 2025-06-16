namespace MainService.Application.Slices.StreamingSlice.DTOs;

public class ForwardStreamingMessageBaseDTO
{
    public string ReceiverConnectionId { get; set; }
    public string CallerConnectionId { get; set; }
}
