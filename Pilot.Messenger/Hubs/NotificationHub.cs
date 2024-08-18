using System.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Pilot.Messenger.Interfaces;

namespace Pilot.Messenger.Hubs;

[Authorize]
public class NotificationHub : Hub<INotificationClient>
{
    public override async Task OnConnectedAsync()
    {
        var name = Context.User!.Identity!.Name ?? throw new NoNullAllowedException("User name is null error");

        await Groups.AddToGroupAsync(Context.ConnectionId, name);

        await base.OnConnectedAsync();
    }
}