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
    public Task<string> Handle(CreateRoomRequest request, CancellationToken cancellationToken)
    {
        roomRepository.AddAsync(new Room
        {
            MeetingId = request.MeetingId,
        }, cancellationToken);
    }
}