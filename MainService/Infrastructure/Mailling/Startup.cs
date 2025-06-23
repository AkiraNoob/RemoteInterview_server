using MainService.Application.Slices.MailSlice.Interfaces;

namespace MainService.Infrastructure.Mailling;

public static class Startup
{
    internal static IServiceCollection AddMailling(this IServiceCollection services, IConfiguration config)
    {
        return services
            .Configure<MailingSettings>(config.GetSection(nameof(MailingSettings)))
            .AddScoped<IMailService, MailService>();
    }
}
