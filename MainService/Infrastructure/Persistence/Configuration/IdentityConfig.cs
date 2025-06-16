using MainService.Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MainService.Infrastructure.Persistence.Configuration;

public class IdentityConfig : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        builder.HasOne(x => x.Avatar).WithOne().HasForeignKey<ApplicationUser>(x => x.AvatarId);
        builder.HasOne(x => x.CV).WithOne().HasForeignKey<ApplicationUser>(x => x.CVId);
        builder.HasOne(x => x.CompanyRegistrationImage).WithOne().HasForeignKey<ApplicationUser>(x => x.CompanyRegistrationImageId);
    }
}
