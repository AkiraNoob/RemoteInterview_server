using Asp.Versioning;
using AuthService.Infrastructure.OpenApi;
using MainService.Infrastructure.Auth;
using MainService.Infrastructure.Cors;
using MainService.Infrastructure.DependencyInjection;
using MainService.Infrastructure.Mailling;
using MainService.Infrastructure.MessageBus;
using MainService.Infrastructure.Middleware;
using MainService.Infrastructure.Persistence;
using MainService.Infrastructure.Persistence.Initialization;
using MainService.Infrastructure.Storage;

namespace AuthService.Infrastructure;

internal static class Startup
{
    internal static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
    {
        services
            .AddResponseCompression()
            .AddApiVersioning()
            .AddAuth(config)
            .AddCorsPolicy(config)
            .AddExceptionMiddleware()
            .AddStorage(config)
            .AddMailling(config)
            .AddOpenApiDocumentation(config)
            .AddPersistence(config)
            .AddRouting(options => options.LowercaseUrls = true)
            .AddServices();

        services.AddSignalR();

        return services;
    }

    private static IServiceCollection AddApiVersioning(this IServiceCollection services) =>
       services.AddApiVersioning(options =>
       {
           options.DefaultApiVersion = new ApiVersion(1);
           options.ReportApiVersions = true;
           options.AssumeDefaultVersionWhenUnspecified = true;
           options.ApiVersionReader = ApiVersionReader.Combine(
               new UrlSegmentApiVersionReader());
       }).AddApiExplorer(options =>
       {
           options.GroupNameFormat = "'v'V";
           options.SubstituteApiVersionInUrl = true;
       }).Services;

    public static async Task InitializeDatabasesAsync(this IServiceProvider services, CancellationToken cancellationToken = default)
    {
        // Create a new scope to retrieve scoped services
        using var scope = services.CreateScope();

        await scope.ServiceProvider.GetRequiredService<IDatabaseInitializer>()
            .InitializeDatabasesAsync(cancellationToken);
    }


    internal static IApplicationBuilder UseInfrastructure(this IApplicationBuilder builder, IConfiguration config) =>
       builder
           .UseResponseCompression()
           .UseExceptionMiddleware()
           .UseRouting()
           .UseCorsPolicy()
           .UseAuthentication()
           .UseCurrentUser()
           .UseAuthorization()
           .UseOpenApiDocumentation(config);

    internal static IEndpointRouteBuilder MapEndpoints(this IEndpointRouteBuilder builder)
    {
        builder.MapControllers().RequireAuthorization();
        return builder;
    }
}
