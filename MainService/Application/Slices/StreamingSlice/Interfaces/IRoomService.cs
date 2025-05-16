using MainService.Application.Interfaces;
using MainService.Application.Slices.StreamingSlice.DTOs;
using MainService.Domain.Models.Streaming;

namespace MainService.Application.Slices.StreamingSlice.Interfaces;

public interface IRoomService : IScopedService
{
    public Task<RoomDTO> GetOrAddRoomAsync(string roomId, CancellationToken cancellationToken = default);
    public Task AddUserToRoomAsync(string userId, string roomId, CancellationToken cancellationToken = default);
    public Task RemoveUserFromRoomAsync(string userId, string roomId, CancellationToken cancellationToken = default);
    public Task<ICollection<Guid>> GetOtherUsersInRoomAsync(string userId, string roomId, CancellationToken = default);
}
