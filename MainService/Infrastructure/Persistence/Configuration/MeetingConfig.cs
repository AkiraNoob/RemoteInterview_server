using MainService.Infrastructure.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using MainService.Domain.Models;

namespace MainService.Infrastructure.Persistence.Configuration;

public class MeetingConfig : IEntityTypeConfiguration<Meeting>
{
    public void Configure(EntityTypeBuilder<Meeting> builder)
    {
        builder.HasMany(x => x.Messages)
               .WithOne(x => x.Meeting)
               .HasForeignKey(m => m.MeetingId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.UserMeetings)
               .WithOne(x => x.Meeting)
               .HasForeignKey(x => x.MeetingId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}