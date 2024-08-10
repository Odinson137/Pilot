using Pilot.Contracts.DTO.ModelDto;
using Pilot.Contracts.Interfaces;
using Pilot.Messenger.Interface;

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