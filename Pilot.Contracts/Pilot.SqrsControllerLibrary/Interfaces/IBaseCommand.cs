namespace Pilot.SqrsControllerLibrary.Interfaces;

public interface IBaseCommand : IBaseUrl
{
    public object ValueDto { get; init; }
    public int UserId { get; init; }
}