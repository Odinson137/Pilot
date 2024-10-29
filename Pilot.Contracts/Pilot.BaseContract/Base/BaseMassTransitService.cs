using MassTransit;
using Microsoft.Extensions.Logging;

namespace Pilot.Contracts.Base;

public class BaseMassTransitService : IBaseMassTransitService
{
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly ILogger<BaseMassTransitService> _logger;

    public BaseMassTransitService(IPublishEndpoint publishEndpoint, ILogger<BaseMassTransitService> logger)
    {
        _publishEndpoint = publishEndpoint;
        _logger = logger;
    }

    public async Task Publish<T>(T model, CancellationToken token = default) where T : class
    {
        _logger.LogInformation($"Publish by {typeof(T).Name}");
        await _publishEndpoint.Publish(model, token);
    }
}