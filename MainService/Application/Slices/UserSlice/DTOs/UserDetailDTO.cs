using MainService.Application.Slices.FileSlice.DTOs;
using MainService.Domain.Enums;

namespace MainService.Application.Slices.UserSlice.DTOs;

public class UserDetailDTO
{
    public string Id { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string FullName { get; set; }
    public string? Description { get; set; }
    public FileDTO? Avatar { get; set; }
    public FileDTO? CV { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public string Address { get; set; }
    public int ProvinceId { get; set; }
    public int DistrictId { get; set; }
    public string? TaxNumber { get; set; }
    public FileDTO? CompanyRegistrationImage { get; set; }
}

public class CompanyProfileDTO
{
    public string TaxNumber { get; set; }
    public FileDTO CompanyRegistrationImage { get; set; }
}

public class ShortenUserDetailDTO
{
    public string FullName { get; set; }
    public string? Avatar { get; set; }
    public string Email { get; set; }
    public string? Description { get; set; }

}