using MainService.Application.Exceptions;
using MainService.Application.Slices.StreamingSlice.DTOs;
using MainService.Application.Slices.StreamingSlice.Interfaces;
using MainService.Application.Slices.UserSlice.Interfaces;
using MainService.Domain.Models.Streaming;
using MainService.Infrastructure.Persistence.Context;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace MainService.Infrastructure.Services;

public class RoomService(
    ApplicationDbContext dbContext,
    IUserService userService
        ) : IRoomService
{
    private readonly ApplicationDbContext _dbContext = dbContext;
    private readonly IUserService _userService = userService;

    public async Task AddUserToRoomAsync(UserToConnectionIdDTO user, string roomId, CancellationToken cancellationToken = default)
    {
        await DoubleCheckRoomAndUser(user.UserId.ToString(), roomId, cancellationToken);

        var isParsingUserIdToGuidSuccess = Guid.TryParse(user.UserId.ToString(), out Guid guidUId);
        if (!isParsingUserIdToGuidSuccess)
        {
            throw new BadRequestException("UserId is invalid");
        }

        var isParsingRoomIdToGuidSuccess = Guid.TryParse(roomId, out Guid guidRId);
        if (!isParsingRoomIdToGuidSuccess)
        {
            throw new BadRequestException("UserId is invalid");
        }

        var roomUser = new RoomUser(guidUId, guidRId, user.ConnectionId);

        await _dbContext.RoomUser.AddAsync(roomUser, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<RoomDTO> GetOrAddRoomAsync(string roomId, CancellationToken cancellationToken = default)
    {
        var room = await _dbContext.Room.Include(x => x.Users).Where(r => r.Id.ToString() == roomId).FirstOrDefaultAsync(cancellationToken);
        if (room == null)
        {
            var newRoom = new Room();
            await _dbContext.AddAsync(newRoom, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return newRoom.Adapt<RoomDTO>();
        }
        return room.Adapt<RoomDTO>();
    }

    public async Task<ICollection<string>> GetOtherUsersInRoomAsync(UserToConnectionIdDTO user, string roomId, CancellationToken cancellationToken = default)
    {
        var room = await CheckAndGetRoomExisted(roomId, cancellationToken);
        var otherUserIds = await _dbContext.RoomUser.Where(ru => ru.UserId.ToString() != user.UserId.ToString()).Select(ru => ru.ConnectionId).ToListAsync(cancellationToken);

        return otherUserIds;
    }

    public async Task<RoomDTO?> RemoveUserFromRoomAsync(UserToConnectionIdDTO user, CancellationToken cancellationToken = default)
    {
        var roomUser = await _dbContext.RoomUser
                            .Where(u => u.ConnectionId == user.ConnectionId && u.UserId == user.UserId)
                            .FirstOrDefaultAsync(cancellationToken);
     
        if(roomUser != null)
        {
            _dbContext.RoomUser.Remove(roomUser);

            var room = await _dbContext.Room.Include(x => x.Users).Where(x => x.Id == roomUser.RoomId).FirstOrDefaultAsync(cancellationToken);

            if(room?.Users.Count == 0)
            {
                _dbContext.Room.Remove(room);
                Log.Information($"Room {room.Id} deleted as no user in room.");
            }

            await _dbContext.SaveChangesAsync(cancellationToken);

            return room.Adapt<RoomDTO>();
        }

        return null;
    }

    private async Task<int> CheckRoomExisted(string roomId, CancellationToken cancellationToken)
    {
        return await _dbContext.Room.Where(r => r.Id.ToString() == roomId).CountAsync(cancellationToken);
    }

    private async Task<RoomDTO> CheckAndGetRoomExisted(string roomId, CancellationToken cancellationToken)
    {
        var room = await _dbContext.Room.Include(x => x.Users).Where(r => r.Id.ToString() == roomId).FirstOrDefaultAsync(cancellationToken);
        return room.Adapt<RoomDTO>();
    }

    private async Task DoubleCheckRoomAndUser(string userId, string roomId, CancellationToken cancellationToken)
    {
        if (await CheckRoomExisted(roomId, cancellationToken) != 1)
        {
            throw new NotFoundException("Room not found");
        }

        if (await _userService.CheckUserExistedAsync(userId.ToString(), cancellationToken) == false)
        {
            throw new NotFoundException("User not found");
        }
    }
}
