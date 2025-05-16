using MainService.Application.Slices.StreamingSlice.Interfaces;
using MainService.Domain.Models.Streaming;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;
using System.Text.RegularExpressions;

namespace MainService.Infrastructure.SignalR;

public class WebRtcSignalingHub : Hub
{
    private readonly IRoomService _roomService;

    public WebRtcSignalingHub(IRoomService roomService)
    {
        _roomService = roomService;
    }

    public async Task JoinRoom(string roomId)
    {
        Console.WriteLine($"User {Context.ConnectionId} attempting to join room {roomId}");
        Context.UserIdentifier

        // Create room if it doesn't exist
        var room = _roomService.GetOrAddRoomAsync(roomId);

        // Add user to the room
        await _roomService.AddUserToRoomAsync(Context.ConnectionId, roomId);
        

        await Groups.AddToGroupAsync(Context.ConnectionId, roomId);
        Console.WriteLine($"User {Context.ConnectionId} joined room {roomId}. Users in room: {string.Join(", ", room.Users)}");

        // Get list of other users already in the room
        var otherUsers = room.Users.Where(id => id != Context.ConnectionId).ToList();

        // Notify the new user about existing users
        await Clients.Caller.SendAsync("existing-users", otherUsers);
        Console.WriteLine($"Sent existing users {string.Join(", ", otherUsers)} to {Context.ConnectionId}");

        // Notify existing users about the new user
        await Clients.Group(roomId).SendAsync("user-joined", Context.ConnectionId);
        Console.WriteLine($"Notified room {roomId} that {Context.ConnectionId} joined.");
    }

    public async Task Offer(string targetUserId, string callerUserId, object signal)
    {
        Console.WriteLine($"Relaying offer from {callerUserId} to {targetUserId}");
        await Clients.Client(targetUserId).SendAsync("offer-received", new
        {
            Signal = signal,
            CallerUserId = callerUserId
        });
    }

    public async Task Answer(string targetUserId, string calleeUserId, object signal)
    {
        Console.WriteLine($"Relaying answer from {calleeUserId} to {targetUserId}");
        await Clients.Client(targetUserId).SendAsync("answer-received", new
        {
            Signal = signal,
            CalleeUserId = calleeUserId
        });
    }

    public async Task IceCandidate(string targetUserId, object candidate, string senderUserId)
    {
        await Clients.Client(targetUserId).SendAsync("ice-candidate-received", new
        {
            Candidate = candidate,
            SenderUserId = senderUserId
        });
    }

    public override async Task OnConnectedAsync()
    {
        Console.WriteLine($"User connected: {Context.ConnectionId}");
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception exception)
    {
        Console.WriteLine($"User disconnecting: {Context.ConnectionId}");

        // Find and remove user from rooms
        foreach (var room in Rooms)
        {
            lock (room.Value.Users)
            {
                if (room.Value.Users.Remove(Context.ConnectionId))
                {
                    Console.WriteLine($"User {Context.ConnectionId} removed from room {room.Key}. Users left: {string.Join(", ", room.Value.Users)}");

                    // Notify remaining users in the room
                    await Clients.Group(room.Key).SendAsync("user-left", Context.ConnectionId);
                    Console.WriteLine($"Notified room {room.Key} that {Context.ConnectionId} left.");

                    // Clean up empty rooms
                    if (room.Value.Users.Count == 0)
                    {
                        Rooms.TryRemove(room.Key, out _);
                        Console.WriteLine($"Room {room.Key} deleted as it is empty.");
                    }
                }
            }
        }

        await base.OnDisconnectedAsync(exception);
    }
}