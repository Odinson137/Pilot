using Microsoft.Extensions.Configuration;
using Pilot.Contracts.Exception.ProjectExceptions;

namespace Pilot.Contracts.Services;

public static class ConfigService
{
    public static string GetConnection(this ConfigurationManager configuration, string url)
    {
        var variable = Environment.GetEnvironmentVariable(url) ?? configuration[url];
            
        if (variable == null) throw new NotFoundException("Значение нигде не найдено");

        return variable;

    }
}