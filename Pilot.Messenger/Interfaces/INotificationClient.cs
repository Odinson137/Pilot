using Pilot.Contracts.DTO.ModelDto;

namespace Pilot.Messenger.Interfaces;

public interface INotificationClient
{
    public Task SendNotificationAsync(InfoMessageDto message);
}