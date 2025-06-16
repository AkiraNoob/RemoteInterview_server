using MainService.Domain.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace MainService.Infrastructure.Persistence.Configuration;

public class RecruitmentConfig : IEntityTypeConfiguration<Recruitment>
{
    public void Configure(EntityTypeBuilder<Recruitment> builder)
    {
       builder.HasMany(x => x.UserRecruitments).WithOne(x => x.Recruitment).HasForeignKey(x => x.UserId);
       builder.HasMany(x => x.Meetings).WithOne(x => x.Recruitment).HasForeignKey(x => x.RecruitmentId);
        builder.HasMany(x => x.RecruitmentTags).WithOne(x => x.Recruitment).HasForeignKey(x => x.RecruitmentId);
    }
}
