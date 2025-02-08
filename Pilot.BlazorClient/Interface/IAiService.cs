using Pilot.BlazorClient.ViewModels;

namespace Pilot.BlazorClient.Interface;

public interface IAiService
{
    Task SendMessageAsync(string prompt, MessageViewModel message, Action continuedMessage);
}