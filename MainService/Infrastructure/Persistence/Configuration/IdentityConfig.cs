using MainService.Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MainService.Infrastructure.Persistence.Configuration;

public class IdentityConfig : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        builder.OwnsOne(x => x.CompanyProfile, cp =>
        {
            cp.HasOne(c => c.CompanyRegistrationImage).WithOne().HasForeignKey<ApplicationUserCompanyProfile>(a => a.CompanyRegistrationImageId);  
        });

        builder.HasOne(x => x.Avatar).WithOne().HasForeignKey<ApplicationUser>(x => x.AvatarId);
        builder.HasOne(x => x.CV).WithOne().HasForeignKey<ApplicationUser>(x => x.CVId);

        //builder.HasMany(x => x.Messages)
        //       .WithOne() 
        //       .HasForeignKey(m => m.SenderId) 
        //       .OnDelete(DeleteBehavior.Cascade);

        //builder.HasMany(x => x.Recruitments)
        //       .WithOne()
        //       .HasForeignKey(x => x.CompanyId);

        //builder.HasMany(x => x.ReceivedReviews)
        //       .WithOne()
        //       .HasForeignKey(x => x.CompanyId);

        //builder.HasMany(x => x.WrittenReviews)
        //       .WithOne()
        //       .HasForeignKey(x => x.ReviewerId);
    }
}
