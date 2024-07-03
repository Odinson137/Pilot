namespace Pilot.Api.Data;

public interface IBaseCommand : IBaseUrl
{
    public object ValueDto { get; init; }
    public string UserId { get; init; }
}