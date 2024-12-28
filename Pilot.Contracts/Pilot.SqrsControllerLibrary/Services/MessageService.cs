using Microsoft.Extensions.Logging;
using Pilot.Contracts.Base;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.Contracts.Interfaces;
using Pilot.SqrsControllerLibrary.RabbitMqMessages;

namespace Pilot.SqrsControllerLibrary.Services;

public class MessageService : IMessageService
{
    private readonly ILogger<MessageService> _logger;
    private readonly IBaseMassTransitService _massTransitService;

    public MessageService(IBaseMassTransitService massTransitService, ILogger<MessageService> logger)
    {
        _massTransitService = massTransitService;
        _logger = logger;
    }

    public async Task SendInfoMessageAsync(InfoMessageDto message, int userId)
    {
        _logger.LogInformation("Send info message by mass transit using publish");
        await _massTransitService.Publish(new CreateCommandMessage<InfoMessageDto>(message, userId));
    }

    public async Task SendMessageChatAsync(MessageDto message)
    {
        _logger.LogInformation("Send message to the chat using publish");
        await _massTransitService.Publish(new CreateCommandMessage<MessageDto>(message, message.UserId));
    }
}