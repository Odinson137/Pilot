namespace Pilot.Contracts.Base;

public class BaseDto : BaseId
{
    public static string GetModelName<T>()
    {
        return typeof(T).Name[..^3];
    }
}