using MainService.Application.Exceptions;
using MainService.Application.Interfaces;
using MainService.Application.Slices.MeetingSlice.DTOs;
using MainService.Application.Slices.RecruitmentSlice.Specifications;
using MainService.Domain.Models;
using Mapster;
using MediatR;

namespace MainService.Application.Slices.MeetingSlice.Requests;

public class CreateMeetingRequest : IRequest<ShortenMeetingDTO>
{
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public Guid RecruitmentId { get; set; }
    public ICollection<Guid> VisitorIds { get; set; } = [];
    public string Description { get; set; }
    public string Title { get; set; }
}

public class CreateMeetingRequestHandler(
    IRepository<UserMeeting> userMeetingRepository,
    IRepository<Meeting> meetingRepository,
    IRepository<Recruitment> recruitmentRepository,
    ICurrentUser currentUser
    ) : IRequestHandler<CreateMeetingRequest, ShortenMeetingDTO>
{
    private readonly IRepository<Meeting> _meetingRepository = meetingRepository;
    private readonly IRepository<UserMeeting> _userMeetingRepository = userMeetingRepository;
    private readonly ICurrentUser _currentUser = currentUser;
    private readonly IRepository<Recruitment> _recruitmentRepository = recruitmentRepository;

    public async Task<ShortenMeetingDTO> Handle(CreateMeetingRequest request, CancellationToken cancellationToken)
    {
        var _ = await _recruitmentRepository.FirstOrDefaultAsync(new GetRecruitmentByIdSpec(request.RecruitmentId), cancellationToken)
            ?? throw new NotFoundException("Recruitment not found.");

        var ownerUserId = _currentUser.GetUserId();

        var meeting = new Meeting
        {
            StartTime = request.StartTime,
            EndTime = request.EndTime,
            RecruitmentId = request.RecruitmentId,
            OwnerId = ownerUserId,
            Description = request.Description,
            Title = request.Title,
        };

        await _meetingRepository.AddAsync(meeting, cancellationToken);

        var userMeetings = new List<UserMeeting>
        {
            new()
            {
                MeetingId = meeting.Id,
                UserId = ownerUserId,
                Role = Domain.Enums.MeetingRoleEnum.Owner,
                Status = Domain.Enums.UserMeetingStatusEnum.Accepted
            }
        };

        if(request.VisitorIds != null && request.VisitorIds.Count > 0)
        {
            foreach (var userId in request.VisitorIds)
            {
                userMeetings.Add(new()
                {
                    MeetingId = meeting.Id,
                    UserId = userId,
                    Role = Domain.Enums.MeetingRoleEnum.Guest,
                });
            }

            await _userMeetingRepository.AddRangeAsync(userMeetings, cancellationToken);
        }

        return meeting.Adapt<ShortenMeetingDTO>();
    }
}