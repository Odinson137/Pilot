using Pilot.Api.Data.Enums;
using Pilot.Contracts.RabbitMqMessages.Message;
using Pilot.Receiver.DTO;

namespace Pilot.Receiver.Interface;

public interface IMessage
{
    public Task SendMessage(Message message);
    public Task SendMessageList(ICollection<Message> messages);
}