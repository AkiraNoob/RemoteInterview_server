using MediatR;

namespace MainService.Application.Slices.ScheduleSlice.Requests;

public class CreateMeetingRequest : IRequest<MeetingDTO>
{
}
