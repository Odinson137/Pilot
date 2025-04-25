using Pilot.BlazorClient.ViewModels;

namespace Pilot.BlazorClient.Interface;

public interface IMessengerService
{
    Task CreateConnectionAsync();

    Task SendMessageAsync(int chatId, string messageText);
    
    event Action<string>? OnMessageReceived;
    event Action<InfoMessageViewModel>? OnReceiveNotification;
    event Action<InfoMessageViewModel>? OnActionNotification;
}