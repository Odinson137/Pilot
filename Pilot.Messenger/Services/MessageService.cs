using Pilot.Contracts.DTO.ModelDto;
using Pilot.Contracts.Interfaces;

namespace Pilot.Messenger.Services;

public class MessageService : IMessageService
{
    public MessageService()
    {
        
    }
    
    public Task SendMessage(MessageDto message)
    {
        throw new NotImplementedException();
    }
}