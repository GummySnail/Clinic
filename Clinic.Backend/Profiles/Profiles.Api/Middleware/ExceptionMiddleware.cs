using System.Net;
using Profiles.Api.Models;

namespace Profiles.Api.Middleware;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            _logger.LogInformation(ex.ToString());

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = GetStatusCode(ex);

            await context.Response.WriteAsync(
                new ErrorModel
                {
                    StatusCode = context.Response.StatusCode,
                    Message = context.Response.StatusCode == (int)HttpStatusCode.InternalServerError
                        ? "Internal server error"
                        : ex.Message
                }.ToString()
            );
        }
    }

    private static int GetStatusCode(Exception ex)
    {
        return ex switch
        {
            _ => (int)HttpStatusCode.InternalServerError
        };
    }
}