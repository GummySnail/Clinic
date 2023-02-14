using Documents.Api.Consumers;
using Documents.Api.Filters;
using Documents.Api.Middleware;
using Documents.Core.Interfaces.Logic;
using Documents.Core.Interfaces.Services;
using Documents.Core.Logic;
using Documents.Infrastructure.Data;
using Documents.Infrastructure.Services;
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
services.AddDbContext<DocumentsDbContext>(opt =>
{
    opt.UseNpgsql(config.GetConnectionString("DefaultConnection"),
        build => build.MigrationsAssembly(typeof(DocumentsDbContext).Assembly.FullName));
});
services.AddScoped<IAzureService, AzureService>();
services.AddScoped<IPdfGenerator, PdfGenerator>();

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
services.AddMassTransit(cfg =>
{
    cfg.SetKebabCaseEndpointNameFormatter();
    cfg.AddDelayedMessageScheduler();
    cfg.AddConsumer<AppointmentResultCreatedConsumer>();
    cfg.AddConsumer<AppointmentResultEditedConsumer>();
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