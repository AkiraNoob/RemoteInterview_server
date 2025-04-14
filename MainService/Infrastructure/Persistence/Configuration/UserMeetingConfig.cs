using MainService.Domain.Models;
using MainService.Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MainService.Infrastructure.Persistence.Configuration;

public class UserMeetingConfig : IEntityTypeConfiguration<UserMeeting>
{
    public void Configure(EntityTypeBuilder<UserMeeting> builder)
    {
        builder.HasOne(x => x.Meeting).WithMany(c => c.UserMeetings).HasForeignKey(c => c.MeetingId).OnDelete(DeleteBehavior.Cascade);
    }
}
