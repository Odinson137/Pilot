using Pilot.Api.Data.Enums;
using Pilot.Contracts.Base;
using Pilot.Contracts.Data.Enums;
using Pilot.Contracts.Models;

namespace Pilot.Receiver.Interface;

public interface IMessage : IBaseRepository<Message>
{
    public Task SendMessage(string title, string desc, MessagePriority priority);
}