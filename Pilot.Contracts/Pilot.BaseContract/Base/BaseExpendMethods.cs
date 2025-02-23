namespace Pilot.Contracts.Base;

public static class BaseExpendMethods
{
    public static string GetModelName<T>() where T : BaseId
    {
        if (typeof(T) == typeof(BaseModel))
            return typeof(T).Name;
        if (typeof(T) == typeof(BaseDto))
            return typeof(T).Name.Replace("Dto", string.Empty);
        throw new System.Exception("Неизвестный тип");
    }
}