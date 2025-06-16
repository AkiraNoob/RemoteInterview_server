namespace MainService.Application.Slices.StreamingSlice.DTOs;

public class RoomDTO 
{
    public Guid Id { get; set; }
    public HashSet<UserToConnectionIdDTO> Users { get; set; }
}

public class UserToConnectionIdDTO
{
    public Guid UserId { get; set; }
    public string ConnectionId { get; set; }
    public UserToConnectionIdDTO(Guid userId, string connectionId)
    {
        UserId = userId;
        ConnectionId = connectionId;
    }
}
