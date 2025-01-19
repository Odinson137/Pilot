using System.Data;
using System.Security.Claims;
using Microsoft.AspNetCore.SignalR;
using Pilot.Contracts.Base;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.Contracts.Interfaces;
using Pilot.Contracts.Services;
using Pilot.Messenger.Interfaces;

namespace Pilot.Messenger.Hubs;

public class NotificationHub : Hub<INotificationClient>
{
    private readonly IRedisService _redisService;
    private readonly ILogger<NotificationHub> _logger;
    private readonly IMessageService _messageService;

    public NotificationHub(
        ILogger<NotificationHub> logger, IRedisService redisService, IMessageService messageService)
    {
        _logger = logger;
        _redisService = redisService;
        _messageService = messageService;
    }

    public override async Task OnConnectedAsync()
    {
        var userId = Context.User?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value
                     ?? throw new NoNullAllowedException("User name is null error");

        await Groups.AddToGroupAsync(Context.ConnectionId, userId);

        await base.OnConnectedAsync();
    }

    public async Task SendMessage(int chatId, string messageText)
    {
        _logger.LogInformation("Start sending message");

        var userId = Context.User?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value
                     ?? throw new NoNullAllowedException("User name is null error");
        if (string.IsNullOrEmpty(userId)) throw new Exception("User is not identified");

        var message = new MessageDto
        {
            Text = messageText,
            UserId = int.Parse(userId),
            Chat = new BaseDto {Id = chatId}
        };
        
        await _messageService.SendMessageChatAsync(message);

        _logger.LogInformation("End sending message");
    }
    
    public async Task GetNotificationsCount()
    {
        _logger.LogInformation("Start getting noti count");

        var userId = Context.User?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value
                     ?? throw new NoNullAllowedException("User name is null error");
        if (string.IsNullOrEmpty(userId)) throw new Exception("User is not identified");

        var count = await _redisService.GetQueueValuesCountAsync<InfoMessageDto>(userId);

        _logger.LogInformation($"Send noti about count: {count}");
        
        await Clients.Caller.ReceiveNotificationCount((int)count);

        _logger.LogInformation("End getting noti about count");
    }
    
    public async Task GetNotifications()
    {
        _logger.LogInformation("Start getting notis");

        var userId = Context.User?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value
                     ?? throw new NoNullAllowedException("User name is null error");
        if (string.IsNullOrEmpty(userId)) throw new Exception("User is not identified");

        var values = await _redisService.GetQueueValuesAsync<InfoMessageDto>(userId);

        _logger.LogInformation("Send notis");
        
        await Clients.Caller.ReceiveNotifications(values.ToJson());

        _logger.LogInformation("End getting notis");
    }
}