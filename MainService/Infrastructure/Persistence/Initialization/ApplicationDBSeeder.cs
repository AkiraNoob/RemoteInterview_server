namespace MainService.Infrastructure.Persistence.Initialization;

using MainService.Domain.Authorization;
using MainService.Domain.Models;
using MainService.Infrastructure.Constants;
using MainService.Infrastructure.Identity;
using MainService.Infrastructure.Persistence.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

internal class ApplicationDbSeeder
{
    private readonly RoleManager<ApplicationRole> _roleManager;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ApplicationDbContext _dbContext;
    private readonly CustomSeederRunner _seederRunner;
    private readonly ILogger<ApplicationDbSeeder> _logger;

    public ApplicationDbSeeder(RoleManager<ApplicationRole> roleManager, UserManager<ApplicationUser> userManager, CustomSeederRunner seederRunner, ILogger<ApplicationDbSeeder> logger, ApplicationDbContext dbContext)
    {
        _roleManager = roleManager;
        _userManager = userManager;
        _seederRunner = seederRunner;
        _logger = logger;
        _dbContext = dbContext;
    }

    public async Task SeedDatabaseAsync(ApplicationDbContext dbContext, CancellationToken cancellationToken)
    {
        await SeedRolesAsync(dbContext);
        await SeedAdminUserAsync();
        await SeedProfessionAsync();
        await _seederRunner.RunSeedersAsync(cancellationToken);
    }

    private async Task SeedRolesAsync(ApplicationDbContext dbContext)
    {
        foreach (string roleName in FSHRoles.DefaultRoles)
        {
            if (await _roleManager.Roles.SingleOrDefaultAsync(r => r.Name == roleName) is not { } role)
            {
                // Create the role
                _logger.LogInformation("Seeding {role} Role", roleName);
                role = new ApplicationRole(roleName, $"{roleName} Role");
                await _roleManager.CreateAsync(role);
            }

            // Assign permissions
            if (roleName == FSHRoles.Basic || roleName == FSHRoles.Company)
            {
                await AssignPermissionsToRoleAsync(dbContext, FSHPermissions.Basic, role);
            }
            else if (roleName == FSHRoles.Admin)
            {
                await AssignPermissionsToRoleAsync(dbContext, FSHPermissions.Admin, role);
            }
        }
    }

    private async Task AssignPermissionsToRoleAsync(ApplicationDbContext dbContext, IReadOnlyList<FSHPermission> permissions, ApplicationRole role)
    {
        var currentClaims = await _roleManager.GetClaimsAsync(role);
        foreach (var permission in permissions)
        {
            if (!currentClaims.Any(c => c.Type == InternalClaims.Permission && c.Value == permission.Name))
            {
                _logger.LogInformation("Seeding {role} Permission '{permission}'.", role.Name, permission.Name);
                dbContext.RoleClaims.Add(new ApplicationRoleClaim
                {
                    RoleId = role.Id,
                    ClaimType = InternalClaims.Permission,
                    ClaimValue = permission.Name,
                    CreatedBy = "ApplicationDbSeeder"
                });
                await dbContext.SaveChangesAsync();
            }
        }
    }

    private async Task SeedAdminUserAsync()
    {
        if (await _userManager.Users.FirstOrDefaultAsync(u => u.Email == UserConstant.Root.EmailAddress) is not { } adminUser)
        {
            adminUser = new ApplicationUser()
            {
                FullName = UserConstant.Root.FullName,
                Email = UserConstant.Root.EmailAddress,
                UserName = UserConstant.Root.EmailAddress,
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                NormalizedEmail = UserConstant.Root.EmailAddress.ToUpperInvariant(),
                NormalizedUserName = UserConstant.Root.EmailAddress.ToUpperInvariant(),
                Password = BCrypt.Net.BCrypt.HashPassword(UserConstant.DefaultPassword),
                Descriptiion = "Default Admin User",
            };

            _logger.LogInformation("Seeding Default Admin User.");
            await _userManager.CreateAsync(adminUser);
        }

        // Assign role to user
        if (!await _userManager.IsInRoleAsync(adminUser, FSHRoles.Admin))
        {
            _logger.LogInformation("Assigning Admin Role to Admin User");
            await _userManager.AddToRoleAsync(adminUser, FSHRoles.Admin);
        }
    }

    private async Task SeedProfessionAsync()
    {
        var professions = new List<string>
        {
            "Kỹ sư phần mềm",
            "Nhà khoa học dữ liệu",
            "Bác sĩ",
            "Y tá",
            "Dược sĩ",
            "Giáo viên",
            "Giảng viên đại học",
            "Luật sư",
            "Kế toán",
            "Kiến trúc sư",
            "Kỹ sư xây dựng",
            "Kỹ sư cơ khí",
            "Thợ điện",
            "Thợ sửa ống nước",
            "Nhà thiết kế đồ họa",
            "Nhà thiết kế UI/UX",
            "Quản lý sản phẩm",
            "Quản lý dự án",
            "Chuyên viên marketing",
            "Nhân viên kinh doanh",
            "Chuyên viên phân tích tài chính",
            "Đầu bếp",
            "Nhân viên pha chế (barista)",
            "Phục vụ nhà hàng",
            "Công nhân xây dựng",
            "Phi công",
            "Tiếp viên hàng không",
            "Cảnh sát",
            "Lính cứu hỏa",
            "Nhà báo",
            "Nhiếp ảnh gia",
            "Bác sĩ thú y",
            "Nha sĩ",
            "Nhà tâm lý học",
            "Biên dịch viên",
            "Thông dịch viên",
            "Nhà khoa học",
            "Kiểm thử phần mềm",
            "Kỹ sư DevOps"
        };

        List<Profession> data = new();

        foreach (var professionName in professions)
        {
            var profession = new Profession(professionName);
            data.Add(profession);
        }

        await _dbContext.AddRangeAsync(data);
        await _dbContext.SaveChangesAsync();
    }
}

