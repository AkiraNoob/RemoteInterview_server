namespace MainService.Application.Slices.MessageSlice.DTOs;

public class NormalChatDTO
{
    public string Content { get; set; }
    public string SenderId { get; set; }
    public string ReceiverId { get; set; }
}

public class NormalChatResponseDTO
{
    public string Content { get; set; }
    public DateTime Timestamp { get; set; }
    public string SenderId { get; set; }
    public string ReceiverId { get; set; }
}