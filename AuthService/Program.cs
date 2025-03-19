using AuthService.Infrastructure;
using Core.Logging;
using Serilog;
using Shared.Infrastructure.Configuration;
using System.Text.Json.Serialization;
using FluentValidation.AspNetCore;
using AuthService.Application;
using Core.JsonConverters;
using FluentValidation;
using System.Reflection;
using AuthService.Infrastructure.Validator;

var builder = WebApplication.CreateBuilder(args);

builder.AddSharedConfiguration();

StaticLogger.EnsureInitialized(builder.Configuration);
Log.Information("AuthService booting up...");

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

app.UseHttpsRedirection();

app.UseInfrastructure(builder.Configuration);
app.MapEndpoints();
app.MapControllers();

app.Run();
