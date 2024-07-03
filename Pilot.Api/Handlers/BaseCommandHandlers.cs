// using MassTransit;
// using MediatR;
// using Pilot.Api.Commands;
// using Pilot.Contracts.Base;
// using Pilot.Contracts.RabbitMqMessages;
//
// namespace Pilot.Api.Handlers;
//
// public class BaseCommandHandlers<TDto> : 
//     IRequestHandler<AddValueCommand<TDto>>,
//     IRequestHandler<UpdateValueCommand<TDto>>,
//     IRequestHandler<DeleteValueCommand<TDto>>
//     where TDto : BaseDto, IRequest
// {
//     private readonly ILogger<BaseCommandHandlers<TDto>> _logger;
//     private readonly IPublishEndpoint _publishEndpoint;
//
//     protected BaseCommandHandlers(
//         ILogger<BaseCommandHandlers<TDto>> logger,
//         IPublishEndpoint publishEndpoint)
//     {
//         _logger = logger;
//         _publishEndpoint = publishEndpoint;
//     }
//     
//     public async Task Handle(AddValueCommand<TDto> request, CancellationToken cancellationToken)
//     {
//         _logger.LogInformation($"{nameof(TDto)} publish for adding value");
//         await _publishEndpoint.Publish(new BaseCommandMessage<TDto>(request.ValueDto, request.UserId), cancellationToken);
//     }
//
//     public async Task Handle(UpdateValueCommand<TDto> request, CancellationToken cancellationToken)
//     {
//         _logger.LogInformation($"{nameof(TDto)} publish for updating value");
//         await _publishEndpoint.Publish(new BaseCommandMessage<TDto>(request.ValueDto, request.UserId), cancellationToken);
//     }
//
//     public async Task Handle(DeleteValueCommand<TDto> request, CancellationToken cancellationToken)
//     {
//         _logger.LogInformation($"{nameof(TDto)} publish for deleting value");
//         await _publishEndpoint.Publish(new BaseCommandMessage<BaseId>(request.ValueDto, request.UserId), cancellationToken);
//     }
// }