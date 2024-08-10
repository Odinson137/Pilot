using Microsoft.AspNetCore.SignalR;
using Pilot.Messenger.Hubs;

namespace Pilot.Messenger.Services;

public class NotificationService
{
    private readonly IHubContext<NotificationHub> _hubContext;
    
    public NotificationService(IHubContext<NotificationHub> hubContext)
    {
        _hubContext = hubContext;
    }
}