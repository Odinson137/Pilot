namespace Pilot.Api.Data;

public interface IBaseCommand : IBaseUrl
{
    public object ValueDto { get; init; }
    public int UserId { get; init; }
}