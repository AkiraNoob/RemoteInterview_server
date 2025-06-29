﻿namespace MainService.Infrastructure.Configuration;

public static class Startup
{
    /// <summary>
    /// this method is used to add every configuration files that appears in every services
    /// </summary>
    public static WebApplicationBuilder AddSharedConfiguration(this WebApplicationBuilder builder)
    {
        const string configurationsDirectory = "Infrastructure/Configuration";

        builder.Configuration
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"{configurationsDirectory}/security.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"{configurationsDirectory}/cloudinary.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"{configurationsDirectory}/cors.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"{configurationsDirectory}/database.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"{configurationsDirectory}/mail.json", optional: false, reloadOnChange: true)
            .AddEnvironmentVariables();

        return builder;
    }

}