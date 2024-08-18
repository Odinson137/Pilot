using Microsoft.Extensions.Configuration;
using Pilot.Contracts.Exception.ProjectExceptions;

namespace Pilot.Contracts.Services;

public static class ConfigService
{
    public static string GetConnection(this IConfiguration configuration, string url)
    {
        var isTest = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Test";
        var variable = isTest
            ? Environment.GetEnvironmentVariable(url) ?? throw new NotFoundException("Добавь меня в тесте")
            : configuration[url] ?? throw new NotFoundException("Добавь меня в файл конфигурации");


        if (variable == null) throw new NotFoundException("Значение нигде не найдено");

        return variable;
    }
}