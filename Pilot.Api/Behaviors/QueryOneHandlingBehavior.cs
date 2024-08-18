using MediatR;
using Pilot.Contracts.Base;
using Pilot.SqrsControllerLibrary.Interfaces;

namespace Pilot.Api.Behaviors;

public class QueryOneHandling<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IQueryOneHandling
{
    private readonly IBaseHttpService _httpService;
    private readonly ILogger<QueryOneHandling<TRequest, TResponse>> _logger;

    public QueryOneHandling(ILogger<QueryOneHandling<TRequest, TResponse>> logger, IBaseHttpService httpService)
    {
        _logger = logger;
        _httpService = httpService;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Query one handling {typeof(TRequest).Name}");

        var result = await _httpService.SendGetMessage<TResponse>(request.Url, cancellationToken);

        _logger.LogInformation($"Query one handed {typeof(TRequest).Name}");
        return result;
    }
}