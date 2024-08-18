using Microsoft.AspNetCore.SignalR;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.Messenger.Hubs;
using Pilot.Messenger.Interfaces;

namespace Pilot.Messenger.Services;

public class NotificationService(IHubContext<NotificationHub, INotificationClient> hubContext) : INotificationService
{
    public Task Notify(int userId, MessageDto message)
    {
        return hubContext.Clients.Group(userId.ToString()).SendNotificationAsync(message);
    }
}