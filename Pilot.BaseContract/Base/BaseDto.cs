namespace Pilot.Contracts.Base;

public class BaseDto : BaseId
{
    public static string GetModelName<T>() => typeof(T).Name[..^3];
}