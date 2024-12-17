using Pilot.Contracts.DTO.ModelDto;

namespace Pilot.Contracts.Interfaces;

public interface IMessageService
{
    public Task SendInfoMessageAsync(InfoMessageDto message);
    
    public Task SendMessageChatAsync(MessageDto message);
}