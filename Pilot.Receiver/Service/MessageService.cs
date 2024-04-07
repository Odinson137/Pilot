using MassTransit;
using Pilot.Contracts.RabbitMqMessages.Message;
using Pilot.Receiver.Interface;

namespace Pilot.Receiver.Service;

public class MessageService : IMessage
{
    private readonly IPublishEndpoint _publishEndpoint;
    
    public MessageService(IPublishEndpoint publishEndpoint)
    {
        _publishEndpoint = publishEndpoint;
    }

    public async Task SendMessage(Message message)
    {
        await _publishEndpoint.Publish(message);
    }

    public async Task SendMessageList(ICollection<Message> messages)
    {
        await _publishEndpoint.Publish(messages);
    }
}