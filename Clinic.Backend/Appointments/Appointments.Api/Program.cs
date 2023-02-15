using Appointments.Api.Filters;
using Appointments.Api.Middleware;
using Appointments.Core.Interfaces;
using Appointments.Infrastructure.Data;
using Appointments.Infrastructure.Services;
using FluentValidation.AspNetCore;
using MassTransit;
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

builder.Services.AddDbContext<AppointmentsDbContext>(opt =>
{
    opt.UseNpgsql(config.GetConnectionString("DefaultConnection"),
        build => build.MigrationsAssembly(typeof(AppointmentsDbContext).Assembly.FullName));
});

services.AddScoped<IAppointmentService, AppointmentService>();

//-- Api

services.AddHttpContextAccessor();
services.AddControllers
(opt =>
    opt.Filters.Add<ValidationFilter>()
);

services.AddMassTransit(cfg =>
{
    cfg.SetKebabCaseEndpointNameFormatter();
    cfg.AddDelayedMessageScheduler();
    cfg.UsingRabbitMq((brc, rbfc) =>
    {
        rbfc.UseInMemoryOutbox();
        rbfc.UseMessageRetry(r =>
        {
            r.Incremental(3, TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(1));
        });

        rbfc.UseDelayedMessageScheduler();
        rbfc.Host("amqp://@rabbitmq:5672",h =>
        {
            h.Username("user");
            h.Password("password");
        });
        rbfc.ConfigureEndpoints(brc);
    });
}).AddMassTransitHostedService();

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

app.UseRouting();

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
    var context = servicesProvider.GetRequiredService<AppointmentsDbContext>();
    await context.Database.MigrateAsync();
}
catch (Exception ex)
{
    var loggerDb = servicesProvider.GetRequiredService<ILogger<Program>>();
    loggerDb.LogError(ex, "Error during database migration");
    throw;
}

await app.RunAsync();