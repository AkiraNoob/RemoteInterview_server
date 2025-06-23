using Ardalis.Result;
using MainService.Application.Exceptions;
using MainService.Application.Helpers;
using MainService.Application.Slices.FileSlice.Interfaces;
using MainService.Application.Slices.UserSlice.DTOs;
using MainService.Application.Slices.UserSlice.Interfaces;
using MainService.Domain.Authorization;
using MainService.Infrastructure.Constants;
using MainService.Infrastructure.Identity;
using MainService.Infrastructure.Persistence.Context;
using MainService.Infrastructure.Persistence.Persistence.Extension;
using Mapster;
using MassTransit.Initializers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace MainService.Infrastructure.Services;

public class UserService(
    //ICacheService cache,
    ApplicationDbContext db,
    UserManager<ApplicationUser> userManager,
    RoleManager<ApplicationRole> roleManager,
    IStorageService storageService) : IUserService
{
    public async Task<string?> GetUserIdAsync(string userIdentifier, CancellationToken cancellationToken = default)
    {
        var user = await db.Users.FilterByUserIdentifier(userIdentifier).FirstOrDefaultAsync(cancellationToken);

        return user?.Id;
    }

    public async Task<bool> HasPermissionAsync(string userId, string permission, CancellationToken cancellationToken = default)
    {
        //var permissions = await cache.GetOrSetAsync(
        //    CacheKey.UserPermission(userId),
        //    () => GetPermissionsAsync(userId, cancellationToken),
        //    cancellationToken: cancellationToken);

        var permissions = await GetPermissionsAsync(userId, cancellationToken);

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

    public async Task<UserDetailDTO> GetUserDetailAsync(string userId, CancellationToken cancellationToken = default)
    {
        var user = await db.Users
            .Include(x => x.Avatar)
            .Include(x => x.CompanyRegistrationImage)
            .Where(x => x.Id == userId)
            .FirstOrDefaultAsync(cancellationToken) ?? throw new NotFoundException("User not found");


        UserDetailDTO response = user.Adapt<UserDetailDTO>();
        if (user.CompanyRegistrationImageId != null)
        {
            var image = await db.File.FirstOrDefaultAsync(x => x.Id == user.CompanyRegistrationImageId, cancellationToken)
                ?? throw new NotFoundException("Company registration image not found");

            response.CompanyRegistrationImage = new Application.Slices.FileSlice.DTOs.FileDTO(
            image.Id,
           image.FileName,
            image.FileUrl,
            image.FileType);
        }

        if (user.AvatarId != null)
        {
            var image = await db.File.FirstOrDefaultAsync(x => x.Id == user.CompanyRegistrationImageId, cancellationToken)
               ?? throw new NotFoundException("Company registration image not found");

            response.Avatar = new Application.Slices.FileSlice.DTOs.FileDTO(
            image.Id,
           image.FileName,
            image.FileUrl,
            image.FileType);
        }

        return response;
    }

    public async Task<ICollection<UserDetailDTO>> GetListUsersAsync(IEnumerable<Guid> listIds, CancellationToken cancellationToken = default)
    {
        var stringList = listIds.Adapt<string>();

        return await db.Users
            .Include(x => x.Avatar)
            .Include(x => x.CompanyRegistrationImage)
            .Where(x => stringList.Contains(x.Id))
            .ProjectToType<UserDetailDTO>()
            .ToListAsync(cancellationToken);
    }

    public async Task<ICollection<UserDetailDTO>> GetListUsersAsync(IEnumerable<string> emailIds, CancellationToken cancellationToken = default)
    {
        return await db.Users
             .Include(x => x.Avatar)
             .Include(x => x.CompanyRegistrationImage)
             .Where(x => emailIds.Contains(x.Email))
             .ProjectToType<UserDetailDTO>()
             .ToListAsync(cancellationToken);
    }

    public async Task<bool> CheckUserExistedAsync(string userId, CancellationToken cancellationToken = default)
    {
        var user = await db.Users.Where(x => x.Id == userId).FirstOrDefaultAsync(cancellationToken: cancellationToken);

        return user != null;
    }

    public async Task<Result<string>> CreateUserAsync(CreateUserDTO payload, CancellationToken cancellationToken = default)
    {
        string? userId = await GetUserIdAsync(payload.Email, cancellationToken);
        if (!userId.IsNullOrEmpty())
        {
            return Result.Conflict("User already exists");
        }

        var user = new ApplicationUser(payload);
        var result = await userManager.CreateAsync(user);

        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
            {
                Log.Error($"❌ Error: {error.Code} - {error.Description}");
            }
            return Result.Error("User creation failed.");
        }

        if (!await userManager.IsInRoleAsync(user, FSHRoles.Basic) && user.Email != UserConstant.Root.EmailAddress)
        {
            await userManager.AddToRoleAsync(user, FSHRoles.Basic);
        }

        return Result.Success(user.Id);
    }

    public async Task<Result<string>> UpdateUserInfoAsync(UpdateUserInfoDTO payload, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByIdAsync(payload.UserId) ?? throw new NotFoundException("User not found");

        if (!user.IsOnboarded)
        {
            user.IsOnboarded = true;
        }

        payload.Adapt(user);

        //user.Update(payload);

        if (payload.Avatar != null)
        {
            var file = new Domain.Models.File();
            var result = await storageService.UploadFileAsync(payload.Avatar, file.Id.ToString(), cancellationToken);
            result.Adapt(file);

            await db.File.AddAsync(file, cancellationToken);

            user.AvatarId = file.Id;
        }

        if (payload.CompanyRegistrationImage != null)
        {
            var userRoles = await userManager.GetRolesAsync(user);

            if (!userRoles.Contains(FSHRoles.Company))
            {
                throw new BadRequestException("User must be in Company role to upload company registration image.");
            }

            var file = new Domain.Models.File();
            var result = await storageService.UploadFileAsync(payload.CompanyRegistrationImage, file.Id.ToString(), cancellationToken);
            result.Adapt(file);

            await db.File.AddAsync(file, cancellationToken);

            user.CompanyRegistrationImageId = file.Id;
        }

        if (payload.CV != null)
        {
            var file = new Domain.Models.File();
            var result = await storageService.UploadFileAsync(payload.CV, file.Id.ToString(), cancellationToken);
            result.Adapt(file);

            await db.File.AddAsync(file, cancellationToken);

            user.CVId = file.Id;
        }

        await userManager.UpdateAsync(user);
        await db.SaveChangesAsync(cancellationToken);

        return Result.Success("Success");
    }

    public async Task<Result<string>> UpdateUserInfoAsync(UpdateUserInfoDTO payload, IFormFile companyImage, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByIdAsync(payload.UserId) ?? throw new NotFoundException("User not found");

        if (!user.IsOnboarded)
        {
            user.IsOnboarded = true;
        }

        payload.Adapt(user);

        //user.Update(payload);

        if (payload.Avatar != null)
        {
            var file = new Domain.Models.File();
            var result = await storageService.UploadFileAsync(payload.Avatar, file.Id.ToString(), cancellationToken);
            result.Adapt(file);

            await db.File.AddAsync(file, cancellationToken);

            user.AvatarId = file.Id;
        }

        if (companyImage != null)
        {
            var userRoles = await userManager.GetRolesAsync(user);

            if (!userRoles.Contains(FSHRoles.Company))
            {
                throw new BadRequestException("User must be in Company role to upload company registration image.");
            }

            var file = new Domain.Models.File();
            var result = await storageService.UploadFileAsync(companyImage, file.Id.ToString(), cancellationToken);
            result.Adapt(file);

            await db.File.AddAsync(file, cancellationToken);

            user.CompanyRegistrationImageId = file.Id;
        }

        if (payload.CV != null)
        {
            var file = new Domain.Models.File();
            var result = await storageService.UploadFileAsync(payload.CV, file.Id.ToString(), cancellationToken);
            result.Adapt(file);

            await db.File.AddAsync(file, cancellationToken);

            user.CVId = file.Id;
        }

        await userManager.UpdateAsync(user);
        await db.SaveChangesAsync(cancellationToken);

        return Result.Success("Success");
    }


    public async Task<Result<string>> RegisterCompanyAsync(string userId, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByIdAsync(userId) ?? throw new NotFoundException("User not found");
        await userManager.AddToRoleAsync(user, FSHRoles.Company);

        return Result.Success("User registered as a company successfully.");
    }
}
