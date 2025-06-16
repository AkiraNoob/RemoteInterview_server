using MainService.Application.Interfaces;
using MainService.Application.Slices.StreamingSlice.DTOs;
using MainService.Domain.Models.Streaming;

namespace MainService.Application.Slices.StreamingSlice.Interfaces;

public interface IRoomService : IScopedService
{
    public Task<RoomDTO> GetOrAddRoomAsync(string roomId, CancellationToken cancellationToken = default);
    public Task AddUserToRoomAsync(UserToConnectionIdDTO user, string roomId, CancellationToken cancellationToken = default);
    public Task<ICollection<string>> GetOtherUsersInRoomAsync(UserToConnectionIdDTO user, string roomId, CancellationToken cancellationToken = default);
    public Task<RoomDTO?> RemoveUserFromRoomAsync(UserToConnectionIdDTO user, CancellationToken cancellationToken = default);
}
