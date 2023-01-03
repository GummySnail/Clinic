using Serilog;
using Serilog.Debugging;

namespace Offices.Api.Configuration;

public static class ConfigureSerilog
{
    public static ILoggingBuilder AddSerilog(this ILoggingBuilder logging, IConfiguration config)
    {
        var logger = new LoggerConfiguration()
            .ReadFrom.Configuration(config)
            .CreateLogger();

        logging.ClearProviders();
        logging.AddSerilog(logger);

        SelfLog.Enable(Console.Error);

        return logging;
    }
}