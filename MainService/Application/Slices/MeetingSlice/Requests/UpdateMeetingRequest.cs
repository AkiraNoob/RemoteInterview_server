using MainService.Application.Exceptions;
using MainService.Application.Interfaces;
using MainService.Application.Slices.MeetingSlice.DTOs;
using MainService.Domain.Models;
using Mapster;
using MediatR;

namespace MainService.Application.Slices.MeetingSlice.Requests;

public class UpdateMeetingRequest : UpdateMeetingDTO, IRequest<ShortenMeetingDTO>
{
    public Guid MeetingId { get; set; }
    public UpdateMeetingRequest(Guid meetingId, UpdateMeetingDTO dto)
    {
        MeetingId = meetingId;

        dto.Adapt(this);
    }
}

public class UpdateMeetingRequestHandler(IRepository<Meeting> meetingRepository) : IRequestHandler<UpdateMeetingRequest, ShortenMeetingDTO>
{
    public async Task<ShortenMeetingDTO> Handle(UpdateMeetingRequest request, CancellationToken cancellationToken)
    {
        var meeting = await meetingRepository.GetByIdAsync(request.MeetingId, cancellationToken)
            ?? throw new NotFoundException("Meeting not found.");

        request.Adapt(meeting);

        await meetingRepository.UpdateAsync(meeting, cancellationToken);

        return meeting.Adapt<ShortenMeetingDTO>();
    }
}