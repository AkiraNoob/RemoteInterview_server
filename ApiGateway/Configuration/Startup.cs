namespace ApiGateway.Configuration;

internal static class Startup
{
    internal static WebApplicationBuilder AddConfiguration(this WebApplicationBuilder builder)
    {
        const string configurationsDirectory = "Configuration";

        builder.Configuration
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"{configurationsDirectory}/ocelot.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"{configurationsDirectory}/cors.json", optional: false, reloadOnChange: true)
            .AddEnvironmentVariables();

        return builder;
    }

}
