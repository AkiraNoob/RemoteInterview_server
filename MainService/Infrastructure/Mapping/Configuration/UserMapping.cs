using MainService.Application.Slices.FileSlice.DTOs;
using MainService.Application.Slices.UserSlice.DTOs;
using MainService.Infrastructure.Identity;
using Mapster;

namespace MainService.Infrastructure.Mapping.Configuration;

public class UserMapping : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<ApplicationUser, UserDetailDTO>()
              .Map(dest => dest.UserId, src => src.Id)
              .Map(dest => dest.Avatar, src => src.Avatar.FileUrl)
              .Map(dest => dest.CV, src => new FileDTO(
                  src.CV.Id,
                  src.CV.FileName,
                  src.CV.FileUrl,
                  src.CV.FileType
                  ))
              .Map(dest => dest.CompanyProfile.CompanyRegistrationImage, src => new FileDTO(
                  src.CompanyProfile.CompanyRegistrationImage.Id, 
                  src.CompanyProfile.CompanyRegistrationImage.FileName,
                  src.CompanyProfile.CompanyRegistrationImage.FileUrl,
                  src.CompanyProfile.CompanyRegistrationImage.FileType
                  ));

        config.NewConfig<ApplicationUser, ShortenUserDetailDTO>()
              .Map(dest => dest.Avatar, src => src.Avatar.FileUrl);

        config.NewConfig<UserDetailDTO, ShortenUserDetailDTO>();
    }
}
