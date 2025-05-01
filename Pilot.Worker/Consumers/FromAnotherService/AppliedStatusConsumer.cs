using MassTransit;
using MediatR;
using Pilot.Contracts.Base;
using Pilot.Contracts.Data.Enums;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.Contracts.DTO.TransferServiceDto;
using Pilot.Contracts.Services;
using Pilot.SqrsControllerLibrary.Commands;
using Pilot.SqrsControllerLibrary.Notifications;
using Pilot.SqrsControllerLibrary.RabbitMqMessages;
using Pilot.Worker.Models;

namespace Pilot.Worker.Consumers.FromAnotherService;

public class AppliedStatusConsumer(ILogger<AppliedStatusConsumer> logger,
    IMediator mediator) : IConsumer<BaseCommandMessage<AppliedStatusDto>>

{
    public async Task Consume(ConsumeContext<BaseCommandMessage<AppliedStatusDto>> context)
    {
        logger.LogInformation($"{nameof(AppliedStatusConsumer)} handle consume");

        var newEmployee = new CompanyUserDto
        {
            UserId = context.Message.Value.UserId,
            Company = new BaseDto {Id = context.Message.Value.CompanyId},
            PostId = context.Message.Value.PostId,
        };
        
        var command = new CreateEntityCommand<CompanyUserDto>(newEmployee, context.Message.UserId);
        var model = await mediator.Send(command, context.CancellationToken);

        await mediator.Publish(new MessageSentNotification
        {
            Message = new InfoMessageDto
            {
                MessagePriority = MessageInfo.Success | MessageInfo.Create,
                EntityType = PilotEnumExtensions.GetModelEnumValue<CompanyUser>(),
                EntityId = model.Id
            },
            UserId = context.Message.UserId
        }, context.CancellationToken);
    }
}