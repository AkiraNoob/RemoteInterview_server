using Core.Domain.Contracts;
using Microsoft.AspNetCore.Identity;

namespace Shared.Infrastructure.Identity;

public class ApplicationUser : IdentityUser, IAuditableEntity
{
    public Guid CreatedBy { get; set; }
    public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
    public DateTime? DateOfBirth { get; set; }
    public Guid? DeletedBy { get; set; }
    public DateTime? DeletedOn { get; set; }
    public string FullName { get; set; }
    public string? ImageUrl { get; set; }
    public Guid LastModifiedBy { get; set; }
    public DateTime? LastModifiedOn { get; set; } = DateTime.UtcNow;
    public string Password { get; set; }
    public string Email { get; set; }
}

