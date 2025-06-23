using MainService.Application.Slices.ProfessionSlice.DTOs;
using MainService.Application.Slices.ProfessionSlice.Requests;
using MainService.Domain.Models;
using Mapster;

namespace MainService.Infrastructure.Mapping.Configuration;

public class ProfessionMapping : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Profession, ProfessionDTO>();
        config.NewConfig<UpdateProfessionDTO, UpdateProfessionRequest>();
    }
}
