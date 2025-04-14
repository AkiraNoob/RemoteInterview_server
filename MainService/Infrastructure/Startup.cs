using Asp.Versioning;
using AuthService.Infrastructure.OpenApi;
using MainService.Infrastructure.Auth;
using MainService.Infrastructure.Cors;
using MainService.Infrastructure.DependencyInjection;
using MainService.Infrastructure.MessageBus;
using MainService.Infrastructure.Middleware;

namespace AuthService.Infrastructure;

internal static class Startup
{
    internal static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
    {
        return services
            .AddResponseCompression()
            .AddApiVersioning()
            .AddAuth(config)
            .AddCorsPolicy(config)
            .AddExceptionMiddleware()
            .AddOpenApiDocumentation(config)
            .AddRouting(options => options.LowercaseUrls = true)
            .AddMessageBus()
            .AddServices();
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
