using Auth.Api.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddCoreServices()
    .AddInfrastructureServices(builder.Configuration)
    .AddApiServices();

var app = builder.Build();

app.AddApplicationConfiguration();
await app.AddDatabaseConfiguration();
await app.RunAsync();