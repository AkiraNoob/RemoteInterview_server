using Microsoft.AspNetCore.Authorization;

namespace Shared.Infrastructure.Auth.Permissions;

public class PermissionRequirement(string permission) : IAuthorizationRequirement
{
    public string Permission { get; private set; } = permission;
}