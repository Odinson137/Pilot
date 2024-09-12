using System.Web;
using AutoMapper;
using Newtonsoft.Json;

namespace Pilot.Contracts.Services;

public static class ShortFormatConverting
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
    
    public static string ToQueryString(this object obj)
    {
        var properties = from p in obj.GetType().GetProperties()
            where p.GetValue(obj, null) != null
            select p.Name + "=" + HttpUtility.UrlEncode(p.GetValue(obj, null)?.ToString());

        return "?" + string.Join("&", properties.ToArray());
    }

    public static TOut Map<TOut>(this object model, IMapper mapper)
    {
        return mapper.Map<TOut>(model);
    }
    
    public static ICollection<TOut> MapList<TOut>(this object model, IMapper mapper)
    {
        return mapper.Map<List<TOut>>(model);
    }
}