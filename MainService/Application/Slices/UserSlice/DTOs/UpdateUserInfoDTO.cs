using MainService.Domain.Enums;
using System.Diagnostics.CodeAnalysis;

namespace MainService.Application.Slices.UserSlice.DTOs;

public class UpdateUserInfoDTO
{
    public string UserId { get; set; }
    public string? FullName { get; set; }
    public string? Description { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Address { get; set; }
    public int? DistrictId { get; set; }
    public int? ProvinceId { get; set; }
    public string? TaxNumber { get; set; }
    public IFormFile? Avatar { get; set; }
    public IFormFile? CompanyRegistrationImage { get; set; }
    public IFormFile? CV { get; set; }
    public EmployerStatusEnum? Status { get; set; }
}
