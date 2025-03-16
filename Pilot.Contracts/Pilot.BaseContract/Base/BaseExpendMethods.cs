namespace Pilot.Contracts.Base;

public static class BaseExpendMethods
{
    public static string GetModelName<T>() where T : BaseId
    {
        if (typeof(T).IsSubclassOf(typeof(BaseModel)))
            return typeof(T).Name;
        if (typeof(T).IsSubclassOf(typeof(BaseDto)))
            return typeof(T).Name.Replace("Dto", string.Empty);
        throw new System.Exception("Неизвестный тип");
    }
    
    public static string GetModelName<T>(T value) where T : BaseId
    {
        var type = value.GetType();
        if (type.IsSubclassOf(typeof(BaseModel)))
            return type.Name;
        if (type.IsSubclassOf(typeof(BaseDto)))
            return type.Name.Replace("Dto", string.Empty);
        throw new System.Exception("Неизвестный тип");
    }
}