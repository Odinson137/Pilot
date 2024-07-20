using MediatR;
using Pilot.Api.Services;

namespace Pilot.Api.Behaviors;

public interface IQueryListHandling
{
    string Url { get; }
}

public class QueryListHandling<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IQueryListHandling
{
    private readonly ILogger<QueryListHandling<TRequest, TResponse>> _logger;
    private readonly IHttpReceiverService _httpService;

    public QueryListHandling(ILogger<QueryListHandling<TRequest, TResponse>> logger, IHttpReceiverService httpService)
    {
        _logger = logger;
        _httpService = httpService;
    }
    
    public async Task<TResponse> Handle(
        TRequest request, 
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Query list handling {typeof(TRequest).Name}");

        var result = await _httpService.SendGetMessages<TResponse>(request.Url, null, cancellationToken);
        
        _logger.LogInformation($"Query list handed {typeof(TRequest).Name}");
        return result;
    }
}