using Pilot.Contracts.DTO.ModelDto;

namespace Pilot.Messenger.Interfaces;

public interface INotificationClient
{
    public Task SendNotificationAsync(InfoMessageDto message);
    public Task SendMessageAsync(MessageDto message);

    public Task ReceiveMessage(string message);
}