using MainService.Application.Exceptions;
using MainService.Application.Interfaces;
using MainService.Application.Slices.MeetingSlice.DTOs;
using MainService.Application.Slices.MeetingSlice.Specifications;
using MainService.Application.Slices.UserSlice.DTOs;
using MainService.Application.Slices.UserSlice.Interfaces;
using MainService.Domain.Models;
using Mapster;
using MediatR;

namespace MainService.Application.Slices.MeetingSlice.Requests;

public class GetMeetingDetailRequest : IRequest<DetailedMeetingDTO>
{
    public Guid MeetingId { get; set; }
}

public class GetMeetingDetailRequestHandler(
    IRepository<Meeting> meetingRepository,
    IUserService userService
    ) : IRequestHandler<GetMeetingDetailRequest, DetailedMeetingDTO>
{
    private readonly IRepository<Meeting> _meetingRepository = meetingRepository;
    private readonly IUserService _userService = userService;

    public async Task<DetailedMeetingDTO> Handle(GetMeetingDetailRequest request, CancellationToken cancellationToken)
    {
        var meeting = await _meetingRepository.FirstOrDefaultAsync(new IncludedGetMeetingByIdSpec(request.MeetingId), cancellationToken) ?? throw new NotFoundException("Meeting not found.");

        DetailedMeetingDTO response = meeting.Adapt<DetailedMeetingDTO>();

        foreach (var user in meeting.UserMeetings)
        {
            if (user.Role == Domain.Enums.MeetingRoleEnum.Owner)
                continue;

            var userInfo = (await _userService.GetUserDetailAsync(user.Id.ToString(), cancellationToken)).Adapt<ShortenUserDetailDTO>();

            response.Participants.Add(userInfo);
        }

        return response;
    }
}