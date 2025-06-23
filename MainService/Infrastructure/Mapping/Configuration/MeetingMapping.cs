using MainService.Application.Slices.MeetingSlice.DTOs;
using MainService.Application.Slices.MeetingSlice.Requests;
using MainService.Domain.Models;
using Mapster;

namespace MainService.Infrastructure.Mapping.Configuration
{
    public class MeetingMapping : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Meeting, ShortenMeetingDTO>();

            config.NewConfig<Meeting, DetailedMeetingDTO>()
                  .Map(dest => dest.RecruitmentTitle, src => src.Recruitment.Title);

            config.NewConfig<UpdateMeetingRequest, Meeting>()
                .IgnoreNullValues(true);

            config.NewConfig<UpdateMeetingDTO, UpdateMeetingRequest>();
        }
    }
}
