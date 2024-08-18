namespace Pilot.SqrsControllerLibrary.Queries;

public record BaseQuery
{
    protected static string GetModelName<T>()
    {
        return typeof(T).Name[..^3];
    }
}