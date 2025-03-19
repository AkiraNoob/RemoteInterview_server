using AuthService.Application;
using AuthService.Infrastructure;
using Core.JsonConverters;
using Core.Logging;
using FluentValidation.AspNetCore;
using Serilog;
using Shared.Infrastructure.Configuration;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.AddSharedConfiguration();

StaticLogger.EnsureInitialized(builder.Configuration);
Log.Information("UserService booting up...");

builder.Host.UseSerilog((_, config) =>
{
    config.WriteTo.Async(x => x.Console()).ReadFrom.Configuration(builder.Configuration);
});

builder.Services.AddControllers().AddJsonOptions(o =>
{
    o.JsonSerializerOptions.Converters.Add(new DateTimeJsonConvertZulu());
    o.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});

builder.Services.AddFluentValidationAutoValidation().AddFluentValidationClientsideAdapters();
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

app.UseHttpsRedirection();

app.UseInfrastructure(builder.Configuration);
app.MapEndpoints();
app.MapControllers();

app.Run();
