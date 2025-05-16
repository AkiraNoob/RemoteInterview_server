namespace MainService.Application.Slices.StreamingSlice.DTOs;

public class RoomDTO 
{
    public Guid Id { get; set; }
    public HashSet<Guid> UserIds { get; set; }
}
