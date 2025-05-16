using MainService.Application.Common.Caching;
using MainService.Application.Exceptions;
using MainService.Application.Slices.UserSlice.DTOs;
using MainService.Application.Slices.UserSlice.Interfaces;
using MainService.Domain.Authorization;
using MainService.Infrastructure.Identity;
using MainService.Infrastructure.Persistence.Context;
using MainService.Infrastructure.Persistence.Persistence.Extension;
using Mapster;
using MassTransit.Initializers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace MainService.Infrastructure.Services;

public class UserService(
    ICacheService cache,
    ApplicationDbContext db,
    UserManager<ApplicationUser> userManager,
    RoleManager<ApplicationRole> roleManager
        ) : IUserService
{
    public async Task<string?> GetUserId(string userIdentifier, CancellationToken cancellationToken = default)
    {
        var user = await db.Users.FilterByUserIdentifier(userIdentifier).FirstOrDefaultAsync(cancellationToken);

        return user?.Id;
    }

    public async Task<bool> HasPermissionAsync(string userId, string permission, CancellationToken cancellationToken = default)
    {
        var permissions = await cache.GetOrSetAsync(
            CacheKey.UserPermission(userId),
            () => GetPermissionsAsync(userId, cancellationToken),
            cancellationToken: cancellationToken);

        return permissions?.Contains(permission) ?? false;
    }

    public async Task<List<string>> GetPermissionsAsync(string userId, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByIdAsync(userId);

        _ = user ?? throw new NotFoundException("User not found");

        var userRoles = await userManager.GetRolesAsync(user);
        var permissions = new List<string>();
        foreach (var role in await roleManager.Roles
            .Where(r => userRoles.Contains(r.Name))
            .ToListAsync(cancellationToken))
        {
            permissions.AddRange(await db.RoleClaims
                .Where(rc => rc.RoleId == role.Id && rc.ClaimType == InternalClaims.Permission)
                .Select(rc => rc.ClaimValue)
                .ToListAsync(cancellationToken));
        }

        return permissions.Distinct().ToList();
    }

    public async Task<UserDetailDTO> GetUserDetail(string userId, CancellationToken cancellationToken = default)
    {
        return await db.Users
            .Include(x => x.Avatar)
            .Include(x => x.CompanyProfile.CompanyRegistrationImage)
            .Where(x => x.Id == userId)
            .ProjectToType<UserDetailDTO>()
            .FirstOrDefaultAsync(cancellationToken) ?? throw new NotFoundException("User not found");
    }

    public async Task<ICollection<UserDetailDTO>> GetListUsers(IEnumerable<Guid> listIds, CancellationToken cancellationToken = default)
    {
        return await db.Users
            .Include(x => x.Avatar)
            .Include(x => x.CompanyProfile.CompanyRegistrationImage)
            .Where(x => listIds.Contains(Guid.Parse(x.Id)))
            .ProjectToType<UserDetailDTO>()
            .ToListAsync(cancellationToken);
    }

    public async Task<ICollection<UserDetailDTO>> GetListUsers(IEnumerable<string> emailIds, CancellationToken cancellationToken = default)
    {
        return await db.Users
             .Include(x => x.Avatar)
             .Include(x => x.CompanyProfile.CompanyRegistrationImage)
             .Where(x => emailIds.Contains(x.Email))
             .ProjectToType<UserDetailDTO>()
             .ToListAsync(cancellationToken);
    }

    public async Task<bool> CheckUserExisted(string userId, CancellationToken cancellationToken = default)
    {
        var user = await db.Users.Where(x => x.Id == userId).FirstOrDefaultAsync(cancellationToken: cancellationToken);

        return user != null;
    }
}
