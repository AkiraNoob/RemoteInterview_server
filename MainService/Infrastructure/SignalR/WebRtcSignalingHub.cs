using MainService.Application.Interfaces;
using MainService.Application.Slices.StreamingSlice.DTOs;
using MainService.Application.Slices.StreamingSlice.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Serilog;

namespace MainService.Infrastructure.SignalR;

[Authorize]
public class WebRtcSignalingHub : Hub
{
    private readonly IRoomService _roomService;
    private readonly ICurrentUser _currentUser;
    public WebRtcSignalingHub(IRoomService roomService, ICurrentUser currentUser)
    {
        _roomService = roomService;
        _currentUser = currentUser;
    }

    public async Task JoinRoom(string roomId)
    {
        var userId = _currentUser.GetUserId();
        Log.Information($"User {Context.ConnectionId} with the id {userId} attempting to join room {roomId}");

        var user = new UserToConnectionIdDTO(userId, Context.ConnectionId);

        // Create room if it doesn't exist
        var room = await _roomService.GetOrAddRoomAsync(roomId);

        // Add user to the room
        await _roomService.AddUserToRoomAsync(user, room.Id.ToString());

        await Groups.AddToGroupAsync(Context.ConnectionId, room.Id.ToString());
        Console.WriteLine($"User {Context.ConnectionId}  with the id {userId} joined room {room.Id.ToString()}. Users in room: {room.Id.ToString()}");

        // Get list of other users already in the room
        var otherUserConnectionIds = _roomService.GetOtherUsersInRoomAsync(user, room.Id.ToString());

        // Notify the new user about existing users
        await Clients.Caller.SendAsync("ExistingUsers", otherUserConnectionIds);
        Console.WriteLine($"Sent existing users {string.Join(", ", otherUserConnectionIds)} to {Context.ConnectionId}");

        // Notify existing users about the new user
        await Clients.Group(room.Id.ToString()).SendAsync("UserJoined", Context.ConnectionId);
        Console.WriteLine($"Notified room {room.Id.ToString()} that {Context.ConnectionId} joined.");
    }

    public async Task Offer(OfferStreamingDTO payload)
    {
        Console.WriteLine($"Relaying offer from {payload.CallerConnectionId} to {payload.ReceiverConnectionId}");
        await Clients.Client(payload.ReceiverConnectionId).SendAsync("OfferReceived", new RoutingOfferReceivedStreamingDTO(payload));
    }

    public async Task Answer(AnswerStreamingDTO payload)
    {
        Console.WriteLine($"Relaying answer from {payload.CallerConnectionId}  to  {payload.ReceiverConnectionId}");
        await Clients.Client(payload.ReceiverConnectionId).SendAsync("AnswerReceived", new RoutingAnswerReceivedStreamingDTO(payload));
    }

    public async Task IceCandidate(IceCandidateDTO payload)
    {
        await Clients.Client(payload.ReceiverConnectionId).SendAsync("IceCandidateReceived", new
        {
            payload.Candidate,
            payload.ReceiverConnectionId
        });
    }

    public string GetConnectionId()
    {
        return Context.ConnectionId;
    }

    public override async Task OnConnectedAsync()
    {
        Console.WriteLine($"User connected: {Context.ConnectionId}");
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception exception)
    {
        var userId = _currentUser.GetUserId();
        Console.WriteLine($"User disconnecting: {Context.ConnectionId} with id {userId}");

        var user = new UserToConnectionIdDTO(userId, Context.ConnectionId);

        var room = await _roomService.RemoveUserFromRoomAsync(user);
        if (room != null)
        {
            Console.WriteLine($"User {Context.ConnectionId} with id {userId} removed from room {room.Id}. Users left: {room.Users.Count}");

            await Clients.Group(room.Id.ToString()).SendAsync("UserLeft", Context.ConnectionId);
            Console.WriteLine($"Notified room {room.Id} that {Context.ConnectionId} left.");
        }

        await base.OnDisconnectedAsync(exception);
    }
}