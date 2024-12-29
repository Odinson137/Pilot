using System.Data;
using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.Contracts.Interfaces;
using Pilot.Contracts.Services;
using Pilot.Messenger.Interfaces;
using Pilot.Messenger.Models;

namespace Pilot.Messenger.Hubs;

public class NotificationHub : Hub<INotificationClient>
{
    private readonly IMessageRepository _messageRepository;
    private readonly IChatRepository _chatRepository;
    private readonly IRedisService _redisService;
    private readonly IMapper _mapper;
    private readonly ILogger<NotificationHub> _logger;

    public NotificationHub(IMessageRepository messageRepository, IMapper mapper, IChatRepository chatRepository,
        ILogger<NotificationHub> logger, IRedisService redisService)
    {
        _messageRepository = messageRepository;
        _mapper = mapper;
        _chatRepository = chatRepository;
        _logger = logger;
        _redisService = redisService;
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

        var chat = await _chatRepository.DbSet.Where(c => c.Id == chatId).Include(c => c.ChatMembers).FirstOrDefaultAsync();
        if (chat == null) throw new Exception("Chat is not found");

        var message = new Message
        {
            Text = messageText,
            UserId = int.Parse(userId),
            Chat = chat
        };

        chat.Messages.Add(message);
        await _chatRepository.SaveAsync();

        var chatMembers = chat.ChatMembers;

        var messageDto = _mapper.Map<MessageDto>(message);
        foreach (var member in chatMembers)
        {
            _logger.LogInformation($"Send message to {member.UserId}");
            await Clients.Group($"{member.UserId}").ReceiveMessage(messageDto.ToJson());
        }

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