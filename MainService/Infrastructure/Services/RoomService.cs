using MainService.Application.Exceptions;
using MainService.Application.Slices.StreamingSlice.DTOs;
using MainService.Application.Slices.StreamingSlice.Interfaces;
using MainService.Application.Slices.UserSlice.Interfaces;
using MainService.Domain.Models.Streaming;
using MainService.Infrastructure.Persistence.Context;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace MainService.Infrastructure.Services;

public class RoomService(
    ApplicationDbContext dbContext,
    IUserService userService
        ) : IRoomService
{
    private readonly ApplicationDbContext _dbContext = dbContext;
    private readonly IUserService _userService = userService;

    public async Task AddUserToRoomAsync(string userId, string roomId, CancellationToken cancellationToken = default)
    {
        await DoubleCheckRoomAndUser(userId, roomId, cancellationToken);

        Guid.TryParse(userId, out Guid guidUId);
        Guid.TryParse(roomId, out Guid guidRId);

        var roomUser = new RoomUser(guidUId, guidRId);

        await _dbContext.RoomUser.AddAsync(roomUser, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<RoomDTO> GetOrAddRoomAsync(string roomId, CancellationToken cancellationToken = default)
    {
        var room = await _dbContext.Room.Where(r => r.Id.ToString() == roomId).FirstOrDefaultAsync(cancellationToken);
        if (room == null)
        {
            var newRoom = new Room();
            await _dbContext.AddAsync(newRoom, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return newRoom.Adapt<RoomDTO>();
        }
        return room.Adapt<RoomDTO>();
    }

    public async Task RemoveUserFromRoomAsync(string userId, string roomId, CancellationToken cancellationToken = default)
    {
        await DoubleCheckRoomAndUser(userId, roomId, cancellationToken);

        var roomUser = await _dbContext.RoomUser.Where(ru => ru.UserId.ToString() == userId).FirstOrDefaultAsync(cancellationToken) ?? throw new BadRequestException("User aren't in this room yet");

        _dbContext.RoomUser.Remove(roomUser);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<ICollection<Guid>> GetOtherUsersInRoomAsync(string userId, string roomId, CancellationToken cancellationToken = default)
    {
        var room = await CheckAndGetRoomExisted(roomId, cancellationToken);
        var otherUserIds = await _dbContext.RoomUser.Where(ru => ru.UserId.ToString() != userId).Select(ru => ru.UserId).ToListAsync(cancellationToken);

        return otherUserIds;
    }

    private async Task<int> CheckRoomExisted(string roomId, CancellationToken cancellationToken)
    {
        return await _dbContext.Room.Where(r => r.Id.ToString() == roomId).CountAsync(cancellationToken);
    }

    private async Task<RoomDTO> CheckAndGetRoomExisted(string roomId, CancellationToken cancellationToken)
    {
        var room = await _dbContext.Room.Where(r => r.Id.ToString() == roomId).FirstOrDefaultAsync(cancellationToken);
        return room.Adapt<RoomDTO>();
    }

    private async Task DoubleCheckRoomAndUser(string userId, string roomId, CancellationToken cancellationToken)
    {
        if (await CheckRoomExisted(roomId, cancellationToken) != 1)
        {
            throw new NotFoundException("Room not found");
        }

        if (await _userService.CheckUserExisted(userId.ToString(), cancellationToken) == false)
        {
            throw new NotFoundException("User not found");
        }
    }
}
