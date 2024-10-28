using Pilot.Contracts.DTO.ModelDto;

namespace Pilot.BlazorClient.Interface;

public interface IMessengerService
{
    Task CreateConnectionAsync(int userId);

    Task SendMessageAsync(int chatId, string messageText);
    
    event Action<string>? OnMessageReceived;
}