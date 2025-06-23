using MainService.Domain.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace MainService.Infrastructure.Persistence.Configuration;

public class RecruitmentKeywordConfig : IEntityTypeConfiguration<RecruitmentKeyword>
{
    public void Configure(EntityTypeBuilder<RecruitmentKeyword> builder)
    {
        builder.HasOne(x => x.Recruitment).WithMany(x => x.RecruitmentKeywords).HasForeignKey(x => x.RecruitmentId);
        builder.HasOne(x => x.Keyword).WithMany().HasForeignKey(x => x.KeywordId);
    }
}
