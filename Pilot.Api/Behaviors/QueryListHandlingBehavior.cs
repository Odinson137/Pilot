// using MediatR;
// using Pilot.Contracts.Base;
// using Pilot.SqrsControllerLibrary.Interfaces;
//
// namespace Pilot.Api.Behaviors;
//
// public class QueryListHandling<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
//     where TRequest : IQueryListHandling
//     where TResponse : ICollection<BaseDto>
// {
//     private readonly IBaseHttpService _httpService;
//     private readonly ILogger<QueryListHandling<TRequest, TResponse>> _logger;
//
//     public QueryListHandling(ILogger<QueryListHandling<TRequest, TResponse>> logger, IBaseHttpService httpService)
//     {
//         _logger = logger;
//         _httpService = httpService;
//     }
//
//     public async Task<ICollection<TResponse>> Handle(
//         TRequest request,
//         RequestHandlerDelegate<ICollection<TResponse>> next,
//         CancellationToken cancellationToken)
//     {
//         _logger.LogInformation($"Query list handling {typeof(TRequest).Name}");
//
//         var result = await _httpService.SendGetMessages<TResponse>(request., cancellationToken);
//
//         _logger.LogInformation($"Query list handed {typeof(TRequest).Name}");
//         return result;
//     }
//
//     public Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
//     {
//         throw new NotImplementedException();
//     }
// }