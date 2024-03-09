using Pilot.Api.Data.Enums;

namespace Pilot.Receiver.Interface;

public interface IMessage
{
    public Task SendMessage(string title, string desc, MessagePriority priority);
}