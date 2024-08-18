using Microsoft.Extensions.Configuration;
using Pilot.Contracts.Exception.ProjectExceptions;

namespace Pilot.Contracts.Services;

public static class ConfigService
{
    public static string GetConnection(this IConfiguration configuration, string url)
    {
        var fullUrl = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Test"
            ? $"{configuration.GetValue<string>(url)}"
            : url;

        var variable = configuration[fullUrl];

        if (variable == null) throw new NotFoundException("Значение нигде не найдено");

        return variable;
    }
}