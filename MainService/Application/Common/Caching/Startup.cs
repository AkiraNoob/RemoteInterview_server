//using EasyCaching.Core.Configurations;
//using EasyCaching.Serialization.SystemTextJson.Configurations;
//using MainService.Application.Common.Caching;
//using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.DependencyInjection;
//using System.Text.Json;
//using System.Text.Json.Serialization;

//namespace Core.Caching;

//public static class Startup
//{
//    public static IServiceCollection AddCaching(this IServiceCollection services, IConfiguration config)
//    {
//        var settings = config.GetSection(nameof(CacheSettings)).Get<CacheSettings>() ?? throw new Exception("CacheSettings not found in configuration.");

//        services.AddEasyCaching(options =>
//        {
//            options.WithSystemTextJson(config =>
//            {
//                config.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
//                config.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
//                config.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
//            }, "json");

//            options.UseRedis(config =>
//            {
//                config.DBConfig.Endpoints.Add(new ServerEndPoint(settings.RedisURL, 6379));
//                config.DBConfig.AllowAdmin = true;
//                config.DBConfig.ConnectionTimeout = 10000;
//                config.DBConfig.SyncTimeout = 10000;
//                config.DBConfig.Database = 0;
//                config.SerializerName = "json";
//            });
//        });

//        services.AddScoped<ICacheService, DistributedCacheService>();

//        return services;
//    }
//}