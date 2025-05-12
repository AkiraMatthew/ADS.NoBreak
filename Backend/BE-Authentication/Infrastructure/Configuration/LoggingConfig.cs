using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace Login.Infra.CrossCutting.IoC.Configuration;

public static class LoggingConfig
{
    public static void ConfigureSerilog(HostBuilderContext context, LoggerConfiguration config)
    {
        config.ReadFrom.Configuration(context.Configuration)
              .Enrich.FromLogContext();
    }

    public static void AddLoggingMiddleware(WebApplication app)
    {
        app.UseMiddleware<LoggingMiddleware>();
        app.UseSerilogRequestLogging();
    }
}
