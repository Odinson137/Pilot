using MassTransit;
using Pilot.Contracts.Base;
using Pilot.Contracts.Data.Enums;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.Contracts.Services;
using Pilot.Messenger.Interfaces;
using Pilot.Messenger.Models;
using Pilot.SqrsControllerLibrary.Commands;
using Pilot.SqrsControllerLibrary.RabbitMqMessages;

namespace Pilot.Messenger.Consumers.FromAnotherService;

public class RevertedStatusConsumer(
    ILogger<RevertedStatusConsumer> logger,
    IChatRepository repository,
    IModelService modelService,
    INotificationService notificationService)
    : IConsumer<RevertStatusCommand>
{
    public async Task Consume(ConsumeContext<RevertStatusCommand> context)
    {
        logger.LogInformation($"{nameof(JobApplicationDto)} update consume");

        var chat = await repository.GetChatAsync(context.Message.ChangerUserId, context.Message.UserId,
            context.CancellationToken);
        
        if (chat == null)
        {
            throw new Exception("Chat is not defined!");
        }
        
        chat.Messages.Add(new Message
        {
            Text =  "Operation canceled due to failure in creating employee",
            UserId = (int)ChatMemberId.System
        });

        await repository.SaveAsync(context.CancellationToken);
    }
}