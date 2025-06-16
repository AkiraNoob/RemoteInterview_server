using AuthService.Infrastructure;
using Serilog;
using System.Text.Json.Serialization;
using FluentValidation.AspNetCore;
using MainService.Infrastructure.Logging;
using MainService.Application;
using MainService.Infrastructure.Configuration;
using MainService.Infrastructure.Common.JsonConverters;
using MainService.Infrastructure.SignalR;

var builder = WebApplication.CreateBuilder(args);

builder.AddSharedConfiguration();

StaticLogger.EnsureInitialized(builder.Configuration);
Log.Information("MainService booting up...");

builder.Host.UseSerilog((_, config) =>
{
    config.WriteTo.Async(x => x.Console()).ReadFrom.Configuration(builder.Configuration);
});

builder.Services.AddControllers().AddJsonOptions(o =>
{
    o.JsonSerializerOptions.Converters.Add(new DateTimeJsonConvertZulu());
    o.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});

builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

await app.Services.InitializeDatabasesAsync();

app.UseHttpsRedirection();

app.UseInfrastructure(builder.Configuration);
app.MapEndpoints();
app.MapHub<WebRtcSignalingHub>("/streaming");

app.Run();
