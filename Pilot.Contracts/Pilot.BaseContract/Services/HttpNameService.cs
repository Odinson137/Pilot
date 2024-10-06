using Pilot.Contracts.Attributes;
using Pilot.Contracts.Base;

namespace Pilot.Contracts.Services;

public static class HttpNameService
{
    public static string GetHttpClientName(Type type)
    {
        var attribute = type.GetCustomAttributes(false).OfType<FromServiceAttribute>().SingleOrDefault();

        if (attribute == null)
            throw new System.Exception("Добавь этой сущности аттрибут, к какому именно сервису она относится");

        return attribute.ServiceName.ToString();
    }

    public static string GetHttpClientName<T>()
    {
        return GetHttpClientName(typeof(T));
    }
}