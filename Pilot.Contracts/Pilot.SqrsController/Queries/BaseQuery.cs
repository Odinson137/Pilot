namespace Pilot.SqrsController.Queries;

public record BaseQuery
{
    protected static string GetModelName<T>() => typeof(T).Name[..^3];
}