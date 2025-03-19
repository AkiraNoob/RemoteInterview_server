using Asp.Versioning;
using AuthService.Infrastructure.MessageBus;
using AuthService.Infrastructure.OpenApi;
using AuthService.Infrastructure.Validator;
using Core.DI.DependencyInjection;
using Shared.Infrastructure.Auth;
using Shared.Infrastructure.Cors;
using Shared.Infrastructure.Middleware;

namespace AuthService.Infrastructure;

internal static class Startup
{
    internal static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
    {
        return services
            .AddResponseCompression()
            .AddApiVersioning()
            .AddJwtAuth(config)
            .AddCorsPolicy(config)
            .AddExceptionMiddleware()
            .AddOpenApiDocumentation(config)
            .AddRouting(options => options.LowercaseUrls = true)
            .AddMessageBus()
            .AddValidator()
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
