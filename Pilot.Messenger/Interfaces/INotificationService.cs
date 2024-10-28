using Pilot.Contracts.DTO.ModelDto;

namespace Pilot.Messenger.Interfaces;

public interface INotificationService
{
    public Task Notify(int userId, InfoMessageDto message);

    public Task SendMessage(int chatId, MessageDto message);
}