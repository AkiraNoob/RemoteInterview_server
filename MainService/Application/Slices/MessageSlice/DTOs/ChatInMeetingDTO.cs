using MainService.Application.Slices.UserSlice.DTOs;

namespace MainService.Application.Slices.MessageSlice.DTOs;

public class ChatInMeetingDTO
{
    public string MeetingId { get; set; }
    public string Content { get; set; }
}

public class ChatInMeetingResponseDTO
{
    public string MeetingId { get; set; }
    public string Content { get; set; }
    public DateTime Timestamp { get; set; }
    public ShortenUserDetailDTO Sender { get; set; }
}