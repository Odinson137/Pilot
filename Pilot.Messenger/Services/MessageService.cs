using Pilot.Contracts.DTO.ModelDto;
using Pilot.Contracts.Interfaces;

namespace Pilot.Messenger.Services;

public class MessageService : IMessageService
{
    public Task SendMessage(MessageDto message)
    {
        // TODO я не знаю, что здесь делать
        throw new NotImplementedException();
    }
}