using Pilot.Contracts.Attributes;
using Pilot.Contracts.Base;

namespace Pilot.Contracts.Services;

public static class HttpNameService
{
    public static string GetHttpClientName<T>()
    {
        var type = typeof(T);
        if (type != typeof(BaseDto))
            throw new System.Exception("Пока не реализовано. И не известно: будет ли вообще реализовано");

        var attribute = type.GetCustomAttributes(false).OfType<FromServiceAttribute>().SingleOrDefault();
        
        if (attribute == null)
            throw new System.Exception("Добавь этой сущности аттрибут, к какому именно сервису она относится");
        
        return attribute.ServiceName.ToString();

    }
    
    public static string GetHttpClientName<T>(T _)
    {
        return GetHttpClientName<T>();
    }
}