using MassTransit;
using Pilot.Contracts.RabbitMqMessages.Message;
using Pilot.Contracts.Services.LogService;
using Pilot.Messenger.Interface;

namespace Pilot.Messenger.Consumers;

public class MessageReceivedConsumer : IConsumer<Message>
{
    private readonly ILogger<MessageReceivedConsumer> _logger;
    private readonly IMessage _message;
    private readonly IPublishEndpoint _publishEndpoint;
    
    public MessageReceivedConsumer(
        ILogger<MessageReceivedConsumer> logger, 
        IMessage message, IPublishEndpoint publishEndpoint)
    {
        _logger = logger;
        _publishEndpoint = publishEndpoint;
        _message = message;
    }

    public async Task Consume(ConsumeContext<Message> context)
    {
        _logger.LogInformation("Message received:");
        _logger.LogClassInfo(context.Message);
        
        await _message.AddMessageAsync(context.Message);

        await _publishEndpoint.Publish(context.Message);
        
        _logger.LogInformation("Message sent to the exchange for further processing");
        throw new NotImplementedException();
    }
}