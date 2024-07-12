// using MassTransit;
// using MediatR;
//
// namespace Pilot.Api.Behaviors;
//
// public interface IHasHandler
// {
//     public bool HasHandler { get; set; }
// }
//
// public class HasHandlerHandlingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IHasHandler
// {
//     private readonly ILogger<HasHandlerHandlingBehavior<TRequest, TResponse>> _logger;
//
//     public HasHandlerHandlingBehavior(ILogger<HasHandlerHandlingBehavior<TRequest, TResponse>> logger, IPublishEndpoint publishEndpoint)
//     {
//         _logger = logger;
//     }
//     
//     public async Task<TResponse> Handle(
//         TRequest request, 
//         RequestHandlerDelegate<TResponse> next,
//         CancellationToken cancellationToken)
//     {
//         if (request.HasHandler)
//         {
//             _logger.LogInformation($"Has handler of {typeof(TRequest).Name}");
//             var response = await next();
//             return response;
//         }
//         
//         _logger.LogInformation($"No handler of {typeof(TRequest).Name}");
//         return (TResponse)request.ValueDto;
//     }
// }