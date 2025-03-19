using Microsoft.Extensions.Configuration;
using Serilog;

namespace Core.Logging;

public static class StaticLogger
{
    public static void EnsureInitialized(IConfiguration configuration = default)
    {
        if (Log.Logger is not Serilog.Core.Logger)
        {
            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .WriteTo.Async(x => x.Console())
                .ReadFrom.Configuration(configuration)
                .CreateLogger();
        }
    }
}