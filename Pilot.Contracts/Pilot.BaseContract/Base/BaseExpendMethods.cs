namespace Pilot.Contracts.Base;

public static class BaseExpendMethods
{
    public static string GetModelName<T>(this T model) where T : BaseDto
    {
        return typeof(T).Name[..^3];
    }

    public static string GetModelName<T>() where T : BaseDto
    {
        return typeof(T).Name[..^3];
    }
}