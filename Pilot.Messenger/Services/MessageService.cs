using Pilot.Contracts.DTO.ModelDto;
using Pilot.Contracts.Interfaces;

namespace Pilot.Messenger.Services;

public class MessageService(ILogger<MessageService> logger) : IMessageService
{
    public Task SendMessage(MessageDto message)
    {
        logger.LogError("Произошла ошибка в мессенджере!!! Такого не должно быть!!! Разобраться как можно скорее!!!");
        // TODO я не знаю, что здесь делать
        return Task.CompletedTask;
    }
}