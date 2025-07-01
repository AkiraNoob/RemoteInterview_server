using MainService.Application.Interfaces;
using MainService.Domain.Models.Streaming;
using MediatR;

namespace MainService.Application.Slices.RoomSlice.Requests;

public class CreateRoomRequest : IRequest<string>
{
    public Guid MeetingId { get; set; }
}

public class CreateRoomRequestHandler(IRepository<Room> roomRepository) : IRequestHandler<CreateRoomRequest, string>
{
    public async Task<string> Handle(CreateRoomRequest request, CancellationToken cancellationToken)
    {
        var room = new Room
        {
            MeetingId = request.MeetingId,
        };

        await roomRepository.AddAsync(room, cancellationToken);

        return room.Id.ToString();
    }
}