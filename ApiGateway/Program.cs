using ApiGateway.Configuration;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.AddConfiguration();
builder.Services.AddOcelot();

var app = builder.Build();
app.UseOcelot().Wait();

app.Run();

