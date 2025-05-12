using MassTransit;
using MediatR;
using Pilot.SqrsControllerLibrary.Commands;

namespace Pilot.Worker.Consumers.ApprovedJobApplicationConsumer;

public class FaultApprovedJobApplicationConsumer(ILogger<FaultApprovedJobApplicationConsumer> logger,
    IMediator mediator) : IConsumer<Fault<ApprovedJobApplicationCommand>>
{
    public async Task Consume(ConsumeContext<Fault<ApprovedJobApplicationCommand>> context)
    {
        logger.LogInformation($"{nameof(FaultApprovedJobApplicationConsumer)} handle consume");

        // var newEmployee = new CompanyUserDto
        // {
        //     UserId = context.Message.Message.UserId,
        //     Company = new BaseDto {Id = context.Message.Message.CompanyId},
        //     PostId = context.Message.Message.PostId,
        // };
        //
        // var command = new CreateEntityCommand<CompanyUserDto>(newEmployee, context.Message.Message.ChangerUserId);
        // var model = await mediator.Send(command, context.CancellationToken);
        //
        // await mediator.Publish(new MessageSentNotification
        // {
        //     Message = new InfoMessageDto
        //     {
        //         MessagePriority = MessageInfo.Success | MessageInfo.Create,
        //         EntityType = PilotEnumExtensions.GetModelEnumValue<CompanyUser>(),
        //         EntityId = model.Id
        //     },
        //     UserId = context.Message.Message.ChangerUserId
        // }, context.CancellationToken);
    }
}