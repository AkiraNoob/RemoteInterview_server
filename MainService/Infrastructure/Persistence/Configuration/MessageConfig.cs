using MainService.Domain.Models;
using MainService.Infrastructure.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace MainService.Infrastructure.Persistence.Configuration;

public class MessageConfig : IEntityTypeConfiguration<MeetingMessage>
{
    public void Configure(EntityTypeBuilder<MeetingMessage> builder)
    {
        builder.HasOne(x => x.Meeting).WithMany(x => x.Messages).HasForeignKey(x => x.MeetingId).OnDelete(DeleteBehavior.Cascade);
    }
}