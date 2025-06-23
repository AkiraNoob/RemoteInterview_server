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
              .Map(dest => dest.PhoneNumber, src => src.PhoneNumber)
              .Map(dest => dest.Avatar, src => src.Avatar == null ? null : src.Avatar.FileUrl)
              .Map(dest => dest.CV, src => src.CV == null ? null : new FileDTO(
                  src.CV.Id,
                  src.CV.FileName,
                  src.CV.FileUrl,
                  src.CV.FileType
                  ))
              .Map(dest => dest.CompanyRegistrationImage, src => src.CompanyRegistrationImage == null ? null : new FileDTO(
                  src.CompanyRegistrationImage.Id, 
                  src.CompanyRegistrationImage.FileName,
                  src.CompanyRegistrationImage.FileUrl,
                  src.CompanyRegistrationImage.FileType
                  ));

        config.NewConfig<ApplicationUser, ShortenUserDetailDTO>()
              .Map(dest => dest.Avatar, src => src.Avatar == null ? null : src.Avatar.FileUrl);

        config.NewConfig<UserDetailDTO, ShortenUserDetailDTO>();

        config.NewConfig<UpdateUserInfoDTO, ApplicationUser>().IgnoreNullValues(true);
    }
}
