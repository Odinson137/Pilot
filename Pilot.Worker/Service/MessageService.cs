using Pilot.Contracts.Base;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.Contracts.Interfaces;

namespace Pilot.Worker.Service;

public class MessageService : IMessageService
{
    private readonly ILogger<MessageService> _logger;
    private readonly IBaseMassTransitService _massTransitService;

    public MessageService(IBaseMassTransitService massTransitService, ILogger<MessageService> logger)
    {
        _massTransitService = massTransitService;
        _logger = logger;
    }

    public async Task SendMessageAsync(MessageDto message)
    {
        _logger.LogInformation("Send message by mass transit using publish");
        await _massTransitService.Publish(message);
    }
}