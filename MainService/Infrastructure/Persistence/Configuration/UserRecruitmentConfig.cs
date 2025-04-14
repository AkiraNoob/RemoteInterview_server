using MainService.Domain.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace MainService.Infrastructure.Persistence.Configuration;

public class UserRecruitmentConfig : IEntityTypeConfiguration<UserRecruitment>
{
    public void Configure(EntityTypeBuilder<UserRecruitment> builder)
    {
        builder.HasOne(x => x.Recruitment).WithMany(x => x.UserRecruitments).HasForeignKey(x => x.RecruitmentId);
        builder.HasOne(x => x.AppliedCv).WithOne().HasForeignKey<UserRecruitment>(x => x.FileId);
    }
}