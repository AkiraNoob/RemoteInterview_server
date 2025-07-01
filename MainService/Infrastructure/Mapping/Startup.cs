using Mapster;
using MapsterMapper;
using System.Reflection;

namespace MainService.Infrastructure.Mapping;

public static class Startup
{
    public static IServiceCollection AddMapping(this IServiceCollection services)
    {
        var config = TypeAdapterConfig.GlobalSettings;
        config.Scan(Assembly.GetExecutingAssembly());

        TypeAdapterConfig.GlobalSettings.Default.PreserveReference(true);

        services.AddSingleton(TypeAdapterConfig.GlobalSettings)
                .AddScoped<IMapper, ServiceMapper>();
        return services;
    }
}
