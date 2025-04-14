using MainService.Domain.Contracts;
using MainService.Domain.Enums;
using MainService.Domain.Models;
using Microsoft.AspNetCore.Identity;
using File = MainService.Domain.Models.File;

namespace MainService.Infrastructure.Identity;

public class ApplicationUser : IdentityUser, IAuditableEntity
{
    public Guid CreatedBy { get; set; }
    public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
    public DateTime? DateOfBirth { get; set; }
    public Guid? DeletedBy { get; set; }
    public DateTime? DeletedOn { get; set; }
    public string FullName { get; set; }
    public string? AvatarId { get; set; }
    public string? CVId { get; set; }
    public Guid LastModifiedBy { get; set; }
    public DateTime? LastModifiedOn { get; set; } = DateTime.UtcNow;
    public string Password { get; set; }
    public string Email { get; set; }
    public string Address { get; set; }
    public int DistrictId { get; set; }
    public int ProvinceId { get; set; }
    public UserRoleEnum Role { get; set; }
    public ApplicationUserCompanyProfile CompanyProfile { get; set; }
    public virtual File Avatar { get; set; }
    public virtual File CV { get; set; }
    //public virtual ICollection<Message> Messages { get; set; }
    //public virtual ICollection<Recruitment> Recruitments { get; set; }
    //public virtual ICollection<Review> WrittenReviews { get; set; }
    //public virtual ICollection<Review> ReceivedReviews { get; set; }
}

public class ApplicationUserCompanyProfile
{
    public string TaxNumber { get; set; }
    public string CompanyRegistrationImageId { get; set; }
    public virtual File CompanyRegistrationImage { get; set; }

}