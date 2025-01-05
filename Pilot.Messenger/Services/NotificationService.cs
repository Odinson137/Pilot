using Microsoft.AspNetCore.SignalR;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.Contracts.Services;
using Pilot.Messenger.Hubs;
using Pilot.Messenger.Interfaces;

namespace Pilot.Messenger.Services;

public class NotificationService(IHubContext<NotificationHub, INotificationClient> hubContext) : INotificationService
{
    public Task Notify(int userId, InfoMessageDto message)
    {
        return hubContext.Clients.Group(userId.ToString()).ReceiveNotification(message.ToJson());
    }
    
    public Task SendMessage(MessageDto message, int toUserId)
    {
        return hubContext.Clients.Group($"{toUserId}").ReceiveMessage(message.ToJson());
    }
}