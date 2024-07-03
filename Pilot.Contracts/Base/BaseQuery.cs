namespace Pilot.Contracts.Base;

public record BaseQuery
{
    protected static string GetModelName<T>() => typeof(T).Name[..^3];
}