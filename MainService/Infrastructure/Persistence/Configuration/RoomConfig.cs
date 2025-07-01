using MainService.Domain.Models;
using MainService.Domain.Models.Streaming;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MainService.Infrastructure.Persistence.Configuration;

public class RoomConfig : IEntityTypeConfiguration<Room>
{
    public void Configure(EntityTypeBuilder<Room> builder)
    {
        builder.HasMany(x => x.Users).WithOne(x => x.Room).HasForeignKey(x => x.RoomId);
        builder.HasOne(x => x.Meeting).WithOne(x => x.Room).HasForeignKey<Room>(x => x.MeetingId);
    }
}
