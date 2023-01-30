using Documents.Api.Filters;
using Documents.Api.Middleware;
using Documents.Core.Interfaces.Services;
using Documents.Infrastructure.Data;
using Documents.Infrastructure.Services;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
services.AddDbContext<DocumentsDbContext>(opt =>
{
    opt.UseNpgsql(config.GetConnectionString("DefaultConnection"),
        build => build.MigrationsAssembly(typeof(DocumentsDbContext).Assembly.FullName));
});
services.AddScoped<IDocumentService, DocumentService>();

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

app.UseCors(policy => policy
    .AllowAnyMethod()
    .AllowAnyHeader()
    .AllowAnyOrigin());
    
app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

//-- Database Configuration

using var scope = app.Services.CreateScope();
var servicesProvider = scope.ServiceProvider;

try
{
    var context = servicesProvider.GetRequiredService<DocumentsDbContext>();
    await context.Database.MigrateAsync();
}
catch (Exception ex)
{
    var loggerDb = servicesProvider.GetRequiredService<ILogger<Program>>();
    loggerDb.LogError(ex, "Error during database migration");
}

await app.RunAsync();