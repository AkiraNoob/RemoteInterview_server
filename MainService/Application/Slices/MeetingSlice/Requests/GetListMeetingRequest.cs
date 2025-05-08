using MainService.Application.Interfaces;
using MainService.Application.Slices.MeetingSlice.DTOs;
using MainService.Application.Slices.MeetingSlice.Specifications;
using MainService.Domain.Models;
using Mapster;
using MediatR;

namespace MainService.Application.Slices.MeetingSlice.Requests;

public class GetListMeetingRequest : IRequest<ICollection<ShortenMeetingDTO>>
{
    public DateTime? StartTime { get; set; }
    public DateTime? EndTime { get; set; }
}

public class GetListMeetingRequestHandler(IRepository<Meeting> meetingRepository) : IRequestHandler<GetListMeetingRequest, ICollection<ShortenMeetingDTO>>
{
    private readonly IRepository<Meeting> _meetingRepository = meetingRepository;

    public async Task<ICollection<ShortenMeetingDTO>> Handle(GetListMeetingRequest request, CancellationToken cancellationToken)
    {
        if(request.StartTime != null && request.EndTime != null)
        {
            return (await _meetingRepository.ListAsync(new GetMeetingInTimeRangeSpec((DateTime)request.StartTime, (DateTime)request.EndTime), cancellationToken)).Adapt<List<ShortenMeetingDTO>>();
        }

        if (request.StartTime != null)
        {
            return (await _meetingRepository.ListAsync(new GetMeetingByStartTimeRangeSpec((DateTime)request.StartTime), cancellationToken)).Adapt<List<ShortenMeetingDTO>>();
        }

        if (request.EndTime != null)
        {
            return (await _meetingRepository.ListAsync(new GetMeetingByEndTimeRangeSpec((DateTime)request.EndTime), cancellationToken)).Adapt<List<ShortenMeetingDTO>>();
        }

        return (await _meetingRepository.ListAsync(cancellationToken)).Adapt<List<ShortenMeetingDTO>>();
    }
}