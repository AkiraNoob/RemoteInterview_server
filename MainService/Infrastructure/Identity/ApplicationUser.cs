using MainService.Application.Slices.UserSlice.DTOs;
using MainService.Domain.Contracts;
using MainService.Domain.Enums;
using MainService.Domain.Models;
using MainService.Infrastructure.Constants;
using Microsoft.AspNetCore.Identity;
using System.Diagnostics.CodeAnalysis;
using File = MainService.Domain.Models.File;

namespace MainService.Infrastructure.Identity;

public class ApplicationUser : IdentityUser, IAuditableEntity
{
    public Guid CreatedBy { get; set; }
    public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
    public DateTime? DateOfBirth { get; set; }
    public Guid? DeletedBy { get; set; }
    public DateTime? DeletedOn { get; set; }
    [AllowNull]
    public string? Descriptiion { get; set; }
    public string? FullName { get; set; }
    [AllowNull]
    public Guid? AvatarId { get; set; }
    [AllowNull]
    public Guid? CVId { get; set; }
    public Guid LastModifiedBy { get; set; }
    public DateTime? LastModifiedOn { get; set; } = DateTime.UtcNow;
    public string? Password { get; set; }
    [AllowNull]
    public string? Address { get; set; }
    [AllowNull]
    public int? DistrictId { get; set; }
    [AllowNull]
    public int? ProvinceId { get; set; }
    [AllowNull]
    public string? TaxNumber { get; set; }
    [AllowNull]
    public Guid? CompanyRegistrationImageId { get; set; }
    public bool EmailConfirmed { get; set; } = true;
    public bool IsOnboarded { get; set; } = false;
    public EmployerStatusEnum EmployerStatus { get; set; } = EmployerStatusEnum.Inactive;
    public virtual File? Avatar { get; set; }
    public virtual File? CV { get; set; }
    public virtual File? CompanyRegistrationImage { get; set; }

    public ApplicationUser(CreateUserDTO payload)
    {
        Email = payload.Email;
        FullName = payload.FullName;
        Password = BCrypt.Net.BCrypt.HashPassword(payload.Password);
        UserName = payload.Email;

        EmailConfirmed = true;
        PhoneNumberConfirmed = true;

        NormalizedEmail = UserConstant.Root.EmailAddress.ToUpperInvariant();
        NormalizedUserName = payload.FullName.ToUpperInvariant();
    }
    public ApplicationUser(string fullName, string email, string password)
    {
        FullName = fullName;
        Email = email;
        UserName = email;
        Password = BCrypt.Net.BCrypt.HashPassword(password);
        EmailConfirmed = true;
        PhoneNumberConfirmed = true;
        NormalizedEmail = email.ToUpperInvariant();
        NormalizedUserName = fullName.ToUpperInvariant();
    }

    public ApplicationUser()
    {
    }
}
