using MassTransit;
using MediatR;
using Pilot.Contracts.Base;
using Pilot.Contracts.Data.Enums;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.Contracts.Services;
using Pilot.SqrsControllerLibrary.Commands;
using Pilot.SqrsControllerLibrary.Notifications;
using Pilot.Worker.Models;

namespace Pilot.Worker.Consumers.ApprovedJobApplicationConsumer;

public class ApprovedJobApplicationConsumer(
    ILogger<ApprovedJobApplicationConsumer> logger,
    IMediator mediator) 
    : IConsumer<ApprovedJobApplicationCommand>
{
    public async Task Consume(ConsumeContext<ApprovedJobApplicationCommand> context)
    {
        logger.LogInformation($"{nameof(ApprovedJobApplicationConsumer)} handle consume");

        var newEmployee = new CompanyUserDto
        {
            UserId = context.Message.UserId,
            Company = new BaseDto {Id = context.Message.CompanyId},
            PostId = context.Message.PostId,
        };
        throw new Exception("Test");
        var command = new CreateEntityCommand<CompanyUserDto>(newEmployee, context.Message.ChangerUserId);
        var model = await mediator.Send(command, context.CancellationToken);

        await mediator.Publish(new MessageSentNotification
        {
            Message = new InfoMessageDto
            {
                MessagePriority = MessageInfo.Success | MessageInfo.Create,
                EntityType = PilotEnumExtensions.GetModelEnumValue<CompanyUser>(),
                EntityId = model.Id
            },
            UserId = context.Message.ChangerUserId
        }, context.CancellationToken);
    }
}