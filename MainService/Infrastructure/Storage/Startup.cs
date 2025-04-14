using MainService.Application.Slices.FileSlice.Interfaces;
using MainService.Infrastructure.Auth.Jwt;

namespace MainService.Infrastructure.Storage;

public static class Startup
{
    public static IServiceCollection AddStorage(this IServiceCollection services, IConfiguration configuration)
    {
        var settings = configuration.GetSection("CloudinarySettings").Get<CloudinarySetting>();

        services.Configure<CloudinarySetting>(c =>
        {
            c.ApiSecret = settings.ApiSecret;
            c.ApiKey = settings.ApiKey;
            c.CloudName = settings.CloudName;
        });

        return services;
    }
}
