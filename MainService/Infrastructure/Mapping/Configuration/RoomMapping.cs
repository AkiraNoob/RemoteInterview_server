using MainService.Application.Slices.RecruitmentSlice.DTOs;
using MainService.Application.Slices.StreamingSlice.DTOs;
using MainService.Domain.Models;
using MainService.Domain.Models.Streaming;
using Mapster;

namespace MainService.Infrastructure.Mapping.Configuration;

public class RoomMapping : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Room, RoomDTO>()
              .Map(dest => dest.Users, src => src.Users.Select(x => new UserToConnectionIdDTO(x.UserId, x.ConnectionId)).ToList());
    }
}
