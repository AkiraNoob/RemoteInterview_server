using MainService.Application.Slices.RecruitmentSlice.DTOs;
using MainService.Application.Slices.UserSlice.DTOs;
using MainService.Domain.Models;
using MainService.Infrastructure.Identity;
using Mapster;

namespace MainService.Infrastructure.Mapping.Configuration;

public class RecruitmentMapping : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Recruitment, RecruitmentDTO>();
    }
}
