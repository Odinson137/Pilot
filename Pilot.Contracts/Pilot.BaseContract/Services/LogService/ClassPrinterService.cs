using System.Collections;
using System.Reflection;
using System.Text;
using Microsoft.Extensions.Logging;

namespace Pilot.Contracts.Services.LogService;


// Посмотреть через полгода и понять, плох ли этот код или нет
public static class ClassPrinterService
{
    public static void LogClassInfo<T1, T2>(
        this ILogger<T1> logger, 
        T2? logClass)
    {
        var builder = new StringBuilder();
        if (logClass == null)
        {
            logger.LogInformation("Empty");
            return;
        }
        var type = typeof(T2);
        
        builder.AppendLine($"{type}:");
        
        CreateMessage(logClass, builder, type);

        logger.LogInformation(builder.ToString());
    }

    private static void CreateMessage<T2>(
        T2 logClass, 
        StringBuilder builder, 
        Type classType,
        string tabs = "")
    {
        if (typeof(IEnumerable).IsAssignableFrom(classType) || classType.IsArray)
        {
            var collection = (ICollection)logClass!;

            builder.AppendLine($"The length of collection is {collection.Count}:");
            
            foreach (var value in collection)
            {
                builder.AppendLine($"New value of {value.GetType()}");
                CreateMessage(value, builder, value.GetType());
                builder.AppendLine();
            }

            return;
        }
        
        var properties = classType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
        
        foreach (var property in properties)
        {
            var value = property.GetValue(logClass);

            builder.Append(tabs);
            
            if (value != null && value.GetType().IsClass && value.GetType() != typeof(string))
            {
                builder.AppendLine($"{property.Name}");
                
                CreateMessage(value, builder, value.GetType(), tabs + "    ");
            }
            else
            {
                builder.AppendLine($"{property.Name} - {value}");
            }
        }
    }
}