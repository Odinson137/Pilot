using Pilot.Contracts.RabbitMqMessages.Message;

namespace Pilot.Messenger.Interface;

public interface IMessage
{
    public Task AddMessageAsync(Message message);
}