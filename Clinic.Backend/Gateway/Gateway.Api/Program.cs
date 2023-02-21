using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

if (env=="Development")
{
    builder.Configuration
        .AddJsonFile("ocelot.Development.json", optional: false, reloadOnChange: true)
        .AddEnvironmentVariables();    
}
else
{
    builder.Configuration
        .AddJsonFile("ocelot.Docker.json", optional: false, reloadOnChange: true)
        .AddEnvironmentVariables();
}

builder.Services.AddOcelot(builder.Configuration);
builder.Services.AddCors();

var app = builder.Build();

app.UseHttpsRedirection();

app.UseRouting();

app.UseCors(opt => opt
    .AllowAnyMethod()
    .AllowAnyHeader()
    .AllowAnyOrigin());

await app.UseOcelot();

app.Run();