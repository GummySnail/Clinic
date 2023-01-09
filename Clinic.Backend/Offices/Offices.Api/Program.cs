using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Offices.Api.Filters;
using Offices.Api.Middleware;
using Offices.Core.Interfaces.Services;
using Offices.Infrastructure.Mapping;
using Offices.Infrastructure.Models;
using Offices.Infrastructure.Services;
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

services.Configure<OfficeDatabaseSettings>(config.GetSection("OfficeDatabase"));

services.AddAutoMapper(typeof(MapperProfile).Assembly);
        
services.AddSingleton<IOfficeService, OfficeService>();

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

await app.RunAsync();