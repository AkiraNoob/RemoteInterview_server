using MainService.Domain.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace MainService.Infrastructure.Persistence.Configuration;

public class RecruitmentTagConfig : IEntityTypeConfiguration<RecruitmentTag>
{
    public void Configure(EntityTypeBuilder<RecruitmentTag> builder)
    {
        builder.HasOne(x => x.Recruitment).WithMany(x => x.RecruitmentTags).HasForeignKey(x => x.RecruitmentId);
        builder.HasOne(x => x.Tag).WithMany().HasForeignKey(x => x.TagId);
    }
}
