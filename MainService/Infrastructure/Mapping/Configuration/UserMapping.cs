using MainService.Application.Slices.FileSlice.DTOs;
using MainService.Application.Slices.UserSlice.DTOs;
using MainService.Infrastructure.Identity;
using Mapster;
using Microsoft.AspNetCore.Identity;

namespace MainService.Infrastructure.Mapping.Configuration;

public class UserMapping() : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<ApplicationUser, UserDetailDTO>()
              .Map(dest => dest.Id, src => src.Id)
              .Map(dest => dest.Email, src => src.Email)
              .Map(dest => dest.PhoneNumber, src => src.PhoneNumber);


        config.NewConfig<ApplicationUser, ShortenUserDetailDTO>()
              .Map(dest => dest.Avatar, src => src.Avatar == null ? null : src.Avatar.FileUrl);

        config.NewConfig<UserDetailDTO, ShortenUserDetailDTO>();

        config.NewConfig<UpdateUserInfoDTO, ApplicationUser>().IgnoreNullValues(true);
    }
}
