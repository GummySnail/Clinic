using System.Text;
using Auth.Api.Filters;
using Auth.Api.Middleware;
using Auth.Core.Entities;
using Auth.Core.Interfaces;
using Auth.Core.Services;
using Auth.Infrastructure.Data;
using Auth.Infrastructure.Services;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
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

services.AddDbContext<AuthDbContext>(opt =>
{
    opt.UseSqlServer(config.GetConnectionString("DefaultConnection"),
        build => build.MigrationsAssembly(typeof(AuthDbContext).Assembly.FullName));
});

services.AddIdentity<Account, IdentityRole>(opt =>
    {
        opt.Password.RequiredLength = 6;
        opt.Password.RequireDigit = false;
        opt.Password.RequireLowercase = false;
        opt.Password.RequireUppercase = false;
        opt.Password.RequireNonAlphanumeric = false;
        opt.Password.RequiredUniqueChars = 0;
        opt.User.RequireUniqueEmail = true;
        opt.SignIn.RequireConfirmedEmail = false;
    })
    .AddEntityFrameworkStores<AuthDbContext>()
    .AddDefaultTokenProviders();

services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(opt =>
{
    //opt.SaveToken = true;
    opt.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = config["JWT:Issuer"],
        ValidateAudience = true,
        ValidAudience = config["JWT:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["JWT:SecretKey"])),
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
    };
    opt.Events = new JwtBearerEvents
    {
        OnAuthenticationFailed = context =>
        {
            if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
            {
                context.Response.Headers.Add("IS-TOKEN-EXPIRED", "true");
            }

            return Task.CompletedTask;
        }
    };
});

services.AddScoped<IAuthService, AuthService>();
services.AddScoped<IEmailService, EmailService>();
services.AddScoped<ITokenService, TokenService>();

//-- Api

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
var serviceProvider = scope.ServiceProvider;

try
{
    var context = serviceProvider.GetRequiredService<AuthDbContext>();
    var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = serviceProvider.GetRequiredService<UserManager<Account>>();

    await context.Database.MigrateAsync();
    await DataSeeder.SetApplicationRoleConfiguration(roleManager);
    await DataSeeder.SetWorkers(userManager);
}
catch (Exception ex)
{
    var loggerDb = serviceProvider.GetRequiredService<ILogger<Program>>();
    loggerDb.LogError(ex, "Error during database migration");
    throw;
}

await app.RunAsync();