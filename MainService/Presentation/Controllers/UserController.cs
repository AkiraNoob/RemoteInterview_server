using MainService.Application.Exceptions;
using MainService.Application.Interfaces;
using MainService.Application.Slices.UserSlice.DTOs;
using MainService.Application.Slices.UserSlice.Interfaces;
using MainService.Domain.Authorization;
using MainService.Domain.Enums;
using MainService.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace MainService.Presentation.Controllers;

public class UserController(IUserService userService, ICurrentUser currentUser, UserManager<ApplicationUser> userManager) : VersionedApiController
{
    [HttpPut("{id}")]
    [EndpointDescription("Update user details.")]
    public async Task<IActionResult> UpdateUserAsync(Guid id, [FromForm] UpdateUserInfoDTO request, CancellationToken cancellationToken)
    {
        if (currentUser.GetUserId() != id)
        {
            throw new ForbiddenException("You are not allowed to change this profile");
        }

        return Ok(await userService.UpdateUserInfoAsync(request, cancellationToken));
    }

    [HttpGet("{id}")]
    [EndpointDescription("Get user details by Id.")]
    public async Task<UserDetailDTO> GetUserByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var result = await userService.GetUserDetailAsync(id.ToString(), cancellationToken);
        return result;
    }

    [HttpPut("{id}/status")]
    [EndpointDescription("Change employer account status.")]
    public async Task<IActionResult> ChangeUserStatusAsync(Guid id, [FromQuery] EmployerStatusEnum status, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByIdAsync(id.ToString()) ?? throw new NotFoundException("User not found");

        var userRoles = await userManager.GetRolesAsync(user);

        if (!userRoles.Contains(FSHRoles.Company))
        {
            throw new BadRequestException("User must be in Company role to be changed this field.");
        }

        var result = await userService.UpdateUserInfoAsync(new UpdateUserInfoDTO
        {
            UserId = id.ToString(),
            Status = status
        }, cancellationToken);

        return Ok(result);
    }

    [HttpPut("{id:guid}/register-company")]
    [EndpointDescription("Register company for user.")]
    public async Task<IActionResult> RegisterCompanyAsync(Guid id, CancellationToken cancellationToken)
    {
        var result = await userService.RegisterCompanyAsync(id.ToString(), cancellationToken);
        return Ok(result);
    }

}
