using MassTransit;

namespace Pilot.Contracts.Base;

public class BaseMassTransitService : IBaseMassTransitService
{
    private readonly IPublishEndpoint _publishEndpoint;

    public BaseMassTransitService(IPublishEndpoint publishEndpoint)
    {
        _publishEndpoint = publishEndpoint;
    }

    public async Task Publish<T>(T model, CancellationToken token = default) where T : class
    {
        await _publishEndpoint.Publish(model, token);
    }
}