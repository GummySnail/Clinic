using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration
    .AddJsonFile("ocelot.json", optional: false, reloadOnChange: true)
    .AddEnvironmentVariables();
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