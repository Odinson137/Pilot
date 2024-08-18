using Newtonsoft.Json;

namespace Pilot.Contracts.Services;

public static class JsonConverting
{
    public static string ToJson<T>(this T data)
    {
        return JsonConvert.SerializeObject(data);
    }
    
    public static T FromJson<T>(this object data)
    {
        return JsonConvert.DeserializeObject<T>(data.ToString()!)!;
    }
    
    public static object FromJson(this object data, Type type)
    {
        return JsonConvert.DeserializeObject(data.ToString()!, type)!;
    }
    
    public static T FromJson<T>(this string data)
    {
        return JsonConvert.DeserializeObject<T>(data)!;
    }
}