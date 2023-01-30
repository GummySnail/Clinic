using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Profiles.Api.Filters;
using Profiles.Api.Middleware;
using Profiles.Core.Interfaces.Services;
using Profiles.Infrastructure.Data;
using Profiles.Infrastructure.Mapping;
using Profiles.Infrastructure.Services;
using Serilog;
using Serilog.Debugging;

var builder = WebApplication.CreateBuilder(args);

var config = builder.Configuration;
var services = builder.Services;

//-- Serilog

var logger = new LoggerConfiguration()
    .ReadFrom.Configuration(config)
    .CreateLogger();

builder.Logging.ClearProviders();
builder.Logging.AddSerilog(logger);

SelfLog.Enable(Console.Error);

//-- Infrastructure

services.AddDbContext<ProfileDbContext>(opt =>
{
    opt.UseSqlServer(config.GetConnectionString("DefaultConnection"),
        build => build.MigrationsAssembly(typeof(ProfileDbContext).Assembly.FullName));
});

services.AddAutoMapper(typeof(MapperProfile).Assembly);
        
services.AddScoped<IProfileService, ProfileService>();

services.AddScoped<IAzureService, AzureService>();

//-- Api

services.AddHttpContextAccessor();
services.AddControllers(opt =>
    opt.Filters.Add<ValidationFilter>()
);
services.AddCors();
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();
services.AddFluentValidation(opt =>
{
    opt.RegisterValidatorsFromAssembly(typeof(Program).Assembly);
});

services.Configure<RouteOptions>(opt => opt.LowercaseUrls = true);
services.Configure<ApiBehaviorOptions>(opt => opt.SuppressModelStateInvalidFilter = true);

var app = builder.Build();

//-- Middlewares

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionMiddleware>();
        
app.UseHttpsRedirection();

app.UseRouting();

app.UseCors(policy => policy
    .AllowAnyHeader()
    .AllowAnyMethod()
    .AllowAnyOrigin());

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

//-- Database Configuration

using var scope = app.Services.CreateScope();
var servicesProvider = scope.ServiceProvider;

try
{
    var context = servicesProvider.GetRequiredService<ProfileDbContext>();
    await context.Database.MigrateAsync();
}
catch (Exception ex)
{
    var loggerDb = servicesProvider.GetRequiredService<ILogger<Program>>();
    loggerDb.LogError(ex, "Error during database migration");
}

await app.RunAsync();