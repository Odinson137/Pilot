using MediatR;
using Pilot.Api.Data;
using Pilot.Api.Services;

namespace Pilot.Api.Behaviors;

public interface IQueryOneHandling : IBaseUrl;

public class QueryOneHandling<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IQueryOneHandling
{
    private readonly ILogger<QueryOneHandling<TRequest, TResponse>> _logger;
    private readonly IHttpReceiverService _httpService;

    public QueryOneHandling(ILogger<QueryOneHandling<TRequest, TResponse>> logger, IHttpReceiverService httpService)
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

        var result = await _httpService.SendGetMessages<TResponse>(request.Url, null, cancellationToken);
        
        _logger.LogInformation($"Query one handed {typeof(TRequest).Name}");
        return result;
    }
}