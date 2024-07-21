using Microsoft.Extensions.Configuration;
using Pilot.Contracts.Exception.ProjectExceptions;

namespace Pilot.Contracts.Services;

public static class ConfigService
{
    public static string GetConnection(this ConfigurationManager configuration, string url)
    {
        var fullUrl = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Test"
            ? $"{configuration.GetValue<string>("ENVIRONMENT")}.{url}"
            : url;
        
        var variable = configuration[fullUrl];
            
        if (variable == null) throw new NotFoundException("Значение нигде не найдено");

        return variable;
    }

    public static string GetProjectName(Type programType)
    {
        return programType.Assembly.GetName().Name?.Split(".")[1] ??
               throw new NotFoundException("Не удалось получить название проекта");
    }
}