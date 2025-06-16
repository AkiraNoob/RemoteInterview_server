using MainService.Application.Slices.UserSlice.Interfaces;
using MainService.Domain.Authorization;
using MediatR;
using Microsoft.AspNetCore.Authorization;

namespace MainService.Infrastructure.Auth.Permissions;

internal class PermissionAuthorizationHandler(ISender mediator, IUserService userService) : AuthorizationHandler<PermissionRequirement>
{
    private readonly ISender _mediator = mediator;
    private readonly IUserService _userService = userService;

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
    {
        if (context.User?.GetUserId() is { } userId &&
            await _userService.HasPermissionAsync(userId, requirement.Permission))
        {
            context.Succeed(requirement);
        }
    }
}