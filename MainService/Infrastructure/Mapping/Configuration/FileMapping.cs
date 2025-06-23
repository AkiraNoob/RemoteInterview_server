using MainService.Application.Slices.FileSlice.DTOs;
using MainService.Domain.Models;
using Mapster;

namespace MainService.Infrastructure.Mapping.Configuration;

public class FileMapping : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Domain.Models.File, UploadFileResultDTO>()
            .Map(dest => dest.FilePublicId, src => src.Id);

        config.NewConfig<UploadFileResultDTO, Domain.Models.File>()
            .Map(dest => dest.Id, src => src.FilePublicId)
            .IgnoreNullValues(true);
    }
}
