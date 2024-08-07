﻿
using Pilot.Contracts.Base;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.Receiver.Interface;

namespace Pilot.Receiver.Repository;

public class MessageService : IMessageService
{
    private readonly ILogger<MessageService> _logger;
    private readonly IBaseMassTransitService _massTransitService;
    public MessageService(IBaseMassTransitService massTransitService, ILogger<MessageService> logger)
    {
        _massTransitService = massTransitService;
        _logger = logger;
    }


    public async Task SendMessage(MessageDto message)
    {
        _logger.LogInformation("Send message by mass transit using publish");
        await _massTransitService.Publish(message);
    }
}