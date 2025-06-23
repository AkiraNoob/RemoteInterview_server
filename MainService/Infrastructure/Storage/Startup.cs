using MainService.Application.Slices.FileSlice.Interfaces;
using MainService.Infrastructure.Auth.Jwt;
using MainService.Infrastructure.Persistence.Context;
using static Org.BouncyCastle.Math.EC.ECCurve;

namespace MainService.Infrastructure.Storage;

public static class Startup
{
    public static IServiceCollection AddStorage(this IServiceCollection services, IConfiguration configuration)
    {

        return services
            .Configure<CloudinarySettings>(configuration.GetSection(nameof(CloudinarySettings)))
            .AddScoped<IStorageService, StorageService>();
    }
}
