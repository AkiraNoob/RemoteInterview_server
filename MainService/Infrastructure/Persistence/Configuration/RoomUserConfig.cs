using MainService.Domain.Models.Streaming;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MainService.Infrastructure.Persistence.Configuration;

public class RoomUserConfig : IEntityTypeConfiguration<RoomUser>
{
    public void Configure(EntityTypeBuilder<RoomUser> builder)
    {
        builder.HasOne(x => x.Room).WithMany(x => x.Users).HasForeignKey(x => x.RoomId);
    }
}
