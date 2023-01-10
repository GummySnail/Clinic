using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Debugging;
using Services.Api.Filters;
using Services.Api.Middleware;
using Services.Core.Interfaces.Services;
using Services.Infrastructure.Data;
using Services.Infrastructure.Services;

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

builder.Services.AddDbContext<ServicesDbContext>(opt =>
{
    opt.UseSqlServer(config.GetConnectionString("DefaultConnection"),
        build => build.MigrationsAssembly(typeof(ServicesDbContext).Assembly.FullName));

});

services.AddScoped<IClinicService, ClinicService>();

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
    var context = servicesProvider.GetRequiredService<ServicesDbContext>();
    await context.Database.MigrateAsync();
    await DataSeeder.SetServiceCategories(context);
}
catch (Exception ex)
{
    var loggerDb = servicesProvider.GetRequiredService<ILogger<Program>>();
    loggerDb.LogError(ex, "Error during database migration");
}

await app.RunAsync();