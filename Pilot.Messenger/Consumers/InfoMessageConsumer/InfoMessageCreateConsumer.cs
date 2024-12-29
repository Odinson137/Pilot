using MassTransit;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.Contracts.Interfaces;
using Pilot.Messenger.Interfaces;
using Pilot.SqrsControllerLibrary.RabbitMqMessages;

namespace Pilot.Messenger.Consumers.InfoMessageConsumer;

public class InfoMessageCreateConsumer(
    ILogger<InfoMessageCreateConsumer> logger,
    IRedisService redisService,
    INotificationService notificationService)
    : IConsumer<CreateCommandMessage<InfoMessageDto>>
{

    public async Task Consume(ConsumeContext<CreateCommandMessage<InfoMessageDto>> context)
    {
        logger.LogInformation("Info message create consume");

        var message = context.Message.Value;
        var userId = context.Message.UserId;

        await redisService.AddQueueValueAsync(userId.ToString(), message);
        
        await notificationService.Notify(userId, message);

        logger.LogInformation("Message consumed");
    }
}