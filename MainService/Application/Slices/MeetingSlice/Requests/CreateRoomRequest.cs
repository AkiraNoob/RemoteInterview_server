//using MainService.Application.Exceptions;
//using MainService.Application.Interfaces;
//using MainService.Domain.Models;
//using MainService.Domain.Models.Streaming;
//using MediatR;

//namespace MainService.Application.Slices.MeetingSlice.Requests;

//public class CreateRoomRequest : IRequest<string>
//{
//    public Guid MeetingId { get; set; }
//}

//public class CreateRoomRequestHandler(IRepository<Meeting> meetingRepository, IRepository<UserMeeting> userMeetingRepository) : IRequestHandler<CreateRoomRequest, string>
//{
//    public async Task<string> Handle(CreateRoomRequest request, CancellationToken cancellationToken)
//    {
//        var meeting = await meetingRepository.GetByIdAsync(request.MeetingId, cancellationToken) ?? throw new NotFoundException("Meeting not found.");

//        var roomId = request.MeetingId;

//        var room = new Room
//        {
//            Id = roomId,
//        };




//    }
//}
