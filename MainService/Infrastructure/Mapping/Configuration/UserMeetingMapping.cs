using MainService.Application.Slices.MeetingSlice.DTOs;
using MainService.Domain.Models;
using Mapster;

namespace MainService.Infrastructure.Mapping.Configuration;

public class UserMeetingMapping : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<UserMeeting, MeetingInvitationDTO>()
            .Map(dest => dest.StartTime, src => src.Meeting.StartTime)
            .Map(dest => dest.EndTime, src => src.Meeting.EndTime)
            .Map(dest => dest.RecruitmenTitle, src => src.Meeting.Recruitment.Title);
    }
}
