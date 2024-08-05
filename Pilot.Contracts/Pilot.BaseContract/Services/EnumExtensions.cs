
using Pilot.Contracts.Base;
using Pilot.Contracts.Data.Enums;

namespace Pilot.Contracts.Services;

public static class PilotEnumExtensions
{
    public static ModelType GetModelEnumValue(string modelName)
    {
        var type = typeof(ModelType);

        var fields = type.GetFields();

        int? value = null;
    
        foreach (var field in fields)
        {
            if (field.FieldType != typeof(ModelType)) continue;
    
            if (modelName != field.Name) continue;
        
            value = (int)(field.GetValue(null) ?? throw new InvalidOperationException("Имя найдено, но значение по имени нельзя получить"));
            break;
        }

        if (value == null)
        {
            throw new InvalidOperationException("Значение в Enum не найдено");
        }

        return (ModelType)value;
    }
    public static ModelType GetModelEnumValue<T>() where T : BaseModel
    {
        return GetModelEnumValue(typeof(T).Name);
    }
}