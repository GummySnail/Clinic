using Offices.Api.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.AddSerilog(builder.Configuration);

builder.Services
    .AddCoreServices()
    .AddInfrastructureServices(builder.Configuration)
    .AddApiServices();
    
var app = builder.Build();

app.AddApplicationConfiguration();

await app.RunAsync();