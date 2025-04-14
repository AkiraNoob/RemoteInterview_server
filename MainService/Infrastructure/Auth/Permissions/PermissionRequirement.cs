using Microsoft.AspNetCore.Authorization;

namespace MainService.Infrastructure.Auth.Permissions;

public class PermissionRequirement(string permission) : IAuthorizationRequirement
{
    public string Permission { get; private set; } = permission;
}