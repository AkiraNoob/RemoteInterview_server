using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Shared.Infrastructure.Persistence.Context;
using TMS.Infrastructure.Persistence.Context;

namespace Shared.Infrastructure.Identity;

public static class Startup
{
    public static IServiceCollection AddIdentity(this IServiceCollection services) =>
        services
            .AddIdentity<ApplicationUser, ApplicationRole>(options =>
                {
                    options.Password.RequiredLength = 6;
                    options.Password.RequireDigit = false;
                    options.Password.RequireLowercase = false;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireUppercase = false;
                })
            .AddEntityFrameworkStores<BaseDbContext>()
            .AddDefaultTokenProviders()
            .Services;
}