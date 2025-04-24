namespace Pilot.BlazorClient.Interface;

public interface IMessengerService
{
    Task CreateConnectionAsync();

    Task SendMessageAsync(int chatId, string messageText);
    
    event Action<string>? OnMessageReceived;
    event Action<string>? OnReceiveNotification;
    event Action<string>? OnActionNotification;
}