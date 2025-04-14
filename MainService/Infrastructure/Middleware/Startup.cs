using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace MainService.Infrastructure.Middleware;

public static class Startup
{
    public static IServiceCollection AddExceptionMiddleware(this IServiceCollection services) =>
        services
            .AddExceptionHandler<GlobalExceptionHandler>()
            .AddProblemDetails();

    public static IApplicationBuilder UseExceptionMiddleware(this IApplicationBuilder app) =>
            app.UseExceptionHandler();
}