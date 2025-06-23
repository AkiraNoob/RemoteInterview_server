using MainService.Application.Slices.RecruitmentSlice.DTOs;
using MainService.Application.Slices.RecruitmentSlice.Requests;
using MainService.Domain.Models;
using Mapster;

namespace MainService.Infrastructure.Mapping.Configuration;

public class RecruitmentMapping : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Recruitment, RecruitmentDTO>()
            .Map(dest => dest.TimeStamp, src => src.CreatedOn)
            .Map(dest => dest.NumberOfApplicant, src => src.UserRecruitments.Count)
            .Map(dest => dest.ProfessionName, src => src.Profession.Name)
            ;

        config.NewConfig<UpdateRecruitmentRequest, Recruitment>().IgnoreNullValues(true);
        config.NewConfig<UpdateRecruitmentDTO, UpdateRecruitmentRequest>();
    }
}
