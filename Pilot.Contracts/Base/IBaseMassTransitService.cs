namespace Pilot.Contracts.Base;

public interface IBaseMassTransitService
{
    Task Publish<T>(T model, CancellationToken token = default) where T : class;
}