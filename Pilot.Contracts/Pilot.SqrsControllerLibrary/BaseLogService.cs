using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Serilog;

namespace Pilot.SqrsControllerLibrary;

public static class BaseLogService
{
    public static void AddBaseLogService<TProgram>(this WebApplicationBuilder builder)
    {
        var assembly = typeof(TProgram).Assembly;

        builder.Logging.ClearProviders();
        builder.Logging.AddSerilog(new LoggerConfiguration()
            .WriteTo.Console()
            // .WriteTo.File("logs/test-log.txt")
            .WriteTo.Debug()
            .CreateLogger());
        builder.Services.AddOpenTelemetry()
            .WithTracing(tracing =>
            {
                tracing
                    .AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation()
                    .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService(assembly.FullName))
                    .SetSampler(new AlwaysOnSampler())
                    .AddJaegerExporter(options =>
                    {
                        options.AgentHost = "jaeger";
                        options.AgentPort = 6831;
                    });
            });
    }
}