using System.Reflection;
using System.Text;
using Microsoft.Extensions.Logging;

namespace Pilot.Contracts.Services.LogService;


// Посмотреть через полгода и понять, плох ли этот код или нет
public static class ClassPrinterService
{
    public static void LogClassInfo<T1, T2>(
        this ILogger<T1> logger, 
        T2 logClass)
    {
        var message = CreateMessage(logClass, new StringBuilder(), typeof(T2));
        logger.LogInformation(message.ToString());
    }

    private static StringBuilder CreateMessage<T2>(
        T2 logClass, 
        StringBuilder builder, 
        IReflect classType,
        string tabs = "")
    {
        var properties = classType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
        
        foreach (var property in properties)
        {
            var value = property.GetValue(logClass);

            builder.Append(tabs);
            
            if (value != null && value.GetType().IsClass)
            {
                builder.AppendLine($"{property.Name}");
                
                CreateMessage(value, builder, value.GetType(), tabs + "    ");
            }
            else
            {
                builder.AppendLine($"{property.Name} - {value}");
            }
        }

        return builder;
    }
}