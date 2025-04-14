using FluentValidation;
using System.Reflection;

namespace MainService.Application;

public static class Startup
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        var assembly = Assembly.GetExecutingAssembly();
        return services
            .AddValidatorsFromAssembly(assembly)
            .AddMediatR(x =>
            {
                x.RegisterServicesFromAssembly(assembly);
            });
    }
}