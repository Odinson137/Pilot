using Pilot.Contracts.DTO.ModelDto;

namespace Pilot.Messenger.Interfaces;

public interface INotificationClient
{
    public Task SendNotificationAsync(InfoMessageDto message);
    public Task SendMessageAsync(MessageDto message);

    public Task ReceiveMessage(string message);
    
    public Task ReceiveNotification(string message);
    
    public Task ReceiveNotifications(string message);
    
    public Task ReceiveNotificationCount(int count);
}