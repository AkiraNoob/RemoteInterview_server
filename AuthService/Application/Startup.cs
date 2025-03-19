using FluentValidation;
using System.Reflection;

namespace AuthService.Application;

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